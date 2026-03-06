using Microsoft.AspNetCore.Authorization;
using System.Text.Json;

namespace FMS_Collection.API.Authorization
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            // SuperAdmin bypasses all permission checks
            var isSuperAdminClaim = context.User.FindFirst("IsSuperAdmin")?.Value;
            if (string.Equals(isSuperAdminClaim, "true", StringComparison.OrdinalIgnoreCase))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            var permissionsClaim = context.User.FindFirst("Permissions")?.Value;
            if (string.IsNullOrWhiteSpace(permissionsClaim))
                return Task.CompletedTask;

            HashSet<string>? permissions;
            try
            {
                permissions = JsonSerializer.Deserialize<HashSet<string>>(permissionsClaim);
            }
            catch
            {
                return Task.CompletedTask;
            }

            if (permissions != null && (permissions.Contains("*") || permissions.Contains(requirement.Permission)))
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
