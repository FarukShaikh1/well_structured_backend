namespace FMS_Collection.Core.Response
{
    public class LoginResponse 
    {
        public Guid? Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? UserName { get; set; }
        public string? EmailAddress { get; set; }
        public string? MobileNumber { get; set; }
        public string? Password { get; set; }
        public DateTime? PasswordLastChangeDate { get; set; }
        public DateTime? LockExpiryDate { get; set; }
        public int? FailedLoginCount { get; set; }
        public bool IsOtpRequired { get; set; }
        public DateTime? SpecialOccasionDate { get; set; }
        public Guid? RoleId { get; set; }
        public string? RoleName { get; set; }
        public bool IsDeleted{ get; set; }
        public bool IsLocked { get; set; }
        public string? ErrorMessage { get; set; }
        //public List<Guid>? AccessibleModuleIds { get; set; }
    }
}
