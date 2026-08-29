using Microsoft.AspNetCore.Http;

namespace FMS_Collection.Core.Request
{
    public class UserRegistrationRequest
    {
        public DateTime DateOfBirth { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string? MobileNumber { get; set; }

        public string? Address { get; set; }

        public IFormFile? ProfilePhoto { get; set; }
    }
}