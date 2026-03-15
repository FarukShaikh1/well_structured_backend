using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace FMS_Collection.API.Authorization
{
    /// <summary>
    /// Dynamically creates authorization policies on-the-fly for any permission string.
    /// This means you never need to pre-register policies — just use [RequirePermission("X.Y")].
    /// </summary>
    public class PermissionPolicyProvider(IOptions<AuthorizationOptions> options) : IAuthorizationPolicyProvider
    {
        private readonly DefaultAuthorizationPolicyProvider _fallbackPolicyProvider = new(options);

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync() =>
            _fallbackPolicyProvider.GetDefaultPolicyAsync();

        public Task<AuthorizationPolicy?> GetFallbackPolicyAsync() =>
            _fallbackPolicyProvider.GetFallbackPolicyAsync();

        public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            // Any policy name that doesn't contain a colon is treated as a permission
            if (!string.IsNullOrEmpty(policyName) && !policyName.Contains(':'))
            {
                var policy = new AuthorizationPolicyBuilder()
                    .AddRequirements(new PermissionRequirement(policyName))
                    .Build();
                return Task.FromResult<AuthorizationPolicy?>(policy);
            }

            return _fallbackPolicyProvider.GetPolicyAsync(policyName);
        }
    }
}
