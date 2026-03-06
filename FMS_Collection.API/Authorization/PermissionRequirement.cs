using Microsoft.AspNetCore.Authorization;

namespace FMS_Collection.API.Authorization
{
    public class PermissionRequirement(string permission) : IAuthorizationRequirement
    {
        public string Permission { get; } = permission;
    }
}
