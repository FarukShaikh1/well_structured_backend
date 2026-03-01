
using FMS_Collection.Core.Common;

namespace FMS_Collection.Core.Response
{
    public class CredentialResponse : CommonResponse
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string SiteName { get; set; }
        public string? SiteUrl { get; set; }
        public string? Notes{ get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
    }
}
