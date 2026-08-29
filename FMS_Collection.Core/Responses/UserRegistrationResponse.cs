using FMS_Collection.Core.Common;

namespace FMS_Collection.Core.Response
{
    public class UserRegistrationResponse : CommonResponse
    {
        public Guid Id { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string? MobileNumber { get; set; }

        public string? Address { get; set; }

        public string? ProfilePhoto { get; set; }

        public bool EmailVerified { get; set; }

        public string Status { get; set; } = string.Empty;

        public string? RejectionReason { get; set; }

        public Guid? ApprovedBy { get; set; }

        public DateTime? ApprovedOn { get; set; }
    }
}