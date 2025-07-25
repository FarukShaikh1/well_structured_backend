using System;

namespace FMS_Collection.Core.Response
{
    public class UserListResponse 
    {
        public Guid? Id { get; set; }
        public string? UserName { get; set; }
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
        public string? Password { get; set; }
        public string? EmailAddress { get; set; }
        public string? MobileNumber { get; set; }
        public bool? IsLocked { get; set; }
        public string? RoleName { get; set; }
        public DateTime? PasswordLastChangedOn { get; set; }

    }
}
