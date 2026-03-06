using FluentValidation;
using FMS_Collection.Core.Request;

namespace FMS_Collection.Application.Validators
{
    public class UserRequestValidator : AbstractValidator<UserRequest>
    {
        public UserRequestValidator()
        {
            RuleFor(x => x.EmailAddress)
                .NotEmpty().WithMessage("Email address is required.")
                .EmailAddress().WithMessage("A valid email address is required.")
                .MaximumLength(200).WithMessage("Email must not exceed 200 characters.");

            RuleFor(x => x.Password)
                .NotEmpty().When(x => x.Id == null, ApplyConditionTo.CurrentValidator)
                .WithMessage("Password is required for new users.")
                .MinimumLength(8).When(x => !string.IsNullOrEmpty(x.Password))
                .WithMessage("Password must be at least 8 characters.")
                .Matches(@"[A-Z]").When(x => !string.IsNullOrEmpty(x.Password))
                .WithMessage("Password must contain at least one uppercase letter.")
                .Matches(@"[0-9]").When(x => !string.IsNullOrEmpty(x.Password))
                .WithMessage("Password must contain at least one number.")
                .Matches(@"[^a-zA-Z0-9]").When(x => !string.IsNullOrEmpty(x.Password))
                .WithMessage("Password must contain at least one special character.");
        }
    }
}
