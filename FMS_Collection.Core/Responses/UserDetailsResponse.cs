namespace FMS_Collection.Core.Response
{
    public class UserDetailsResponse 
    {
        public Guid? Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? UserName { get; set; }
        public string? EmailAddress { get; set; }
        public string? Password { get; set; }
        public DateTime? PasswordLastChangeOn { get; set; }
        public Guid? RoleId { get; set; }
        public DateTime? specialOccasionDate{ get; set; }
        public string? MobileNumber { get; set; }
        public string? AlternateNumber { get; set; }
        public string? Address{ get; set; }
        public bool? IsDeleted { get; set; }

    }
}
