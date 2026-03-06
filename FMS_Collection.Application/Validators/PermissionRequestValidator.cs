using FluentValidation;
using FMS_Collection.Core.Request;

namespace FMS_Collection.Application.Validators
{
    public class PermissionRequestValidator : AbstractValidator<PermissionRequest>
    {
        public PermissionRequestValidator()
        {
            RuleFor(x => x.PermissionName)
                .NotEmpty().WithMessage("Permission name is required.")
                .Matches(@"^[A-Za-z]+\.[A-Za-z]+$")
                .WithMessage("Permission name must follow the format 'Module.Action' (e.g. 'User.Create').")
                .MaximumLength(100).WithMessage("Permission name must not exceed 100 characters.");

            RuleFor(x => x.Module)
                .NotEmpty().WithMessage("Module is required.")
                .MaximumLength(100).WithMessage("Module must not exceed 100 characters.");
        }
    }
}
