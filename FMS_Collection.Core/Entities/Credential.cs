using FMS_Collection.Core.Common;

namespace FMS_Collection.Core.Entities
{
    public class Credential : CommonResponse
    {
        public Guid? Id { get; set; }
        public Guid UserId { get; set; }
        public string SiteName { get; set; }
        public string? SiteUrl { get; set; }
        public string UserName { get; set; }
        public string SecretType { get; set; } = "Password";
        public string Password { get; set; }
        public string? Notes { get; set; }
    }
}