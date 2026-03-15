using Microsoft.AspNetCore.Authorization;

namespace FMS_Collection.API.Authorization
{
    /// <summary>
    /// Requires a specific permission claim. Automatically registered as a named policy.
    /// Usage: [RequirePermission("User.Create")]
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class RequirePermissionAttribute(string permission) : AuthorizeAttribute(permission)
    {
        public string Permission { get; } = permission;
    }
}
