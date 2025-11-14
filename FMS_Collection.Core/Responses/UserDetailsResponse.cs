using FMS_Collection.Core.Common;

namespace FMS_Collection.Core.Response
{
    public class UserDetailsResponse : CommonResponse
    {
        public Guid? Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? EmailAddress { get; set; }
        public Guid? RoleId { get; set; }
        public DateOnly? Birthdate { get; set; }
        public string? MobileNumber { get; set; }
        public int? FailedLoginCount { get; set; }
        public DateTime? LockExpiryDate { get; set; }
        public DateTime? PasswordLastChangedOn { get; set; }
        public string? ThumbnailPath { get; set; }
        public string? OriginalPath { get; set; }
        public string? Address { get; set; }
        public bool? IsLocked{ get; set; }
    }
}
