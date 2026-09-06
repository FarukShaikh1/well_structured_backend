namespace FMS_Collection.Core.Request
{
    // Add user will work only from SpecialOccasions
    // and it will not be used in the normal user creation process.
    // only password and roles can be updated for any user
    public class UserRequest
    {
        public Guid? Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MobileNumber { get; set; }
        public string? EmailAddress { get; set; }
        public string? Password { get; set; }
    }
}
