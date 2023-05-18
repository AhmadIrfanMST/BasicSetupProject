using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace WebApplication3.Permission
{
    internal class PermissionPolicyProvider : IAuthorizationPolicyProvider
    {
        //fallback policy triggered when no authorize attribute is present neither on controller nor on an action
        //default policy is triggered when there is an authorize attribute without a policy
        public DefaultAuthorizationPolicyProvider MyPolicyProvider { get; }
        public PermissionPolicyProvider(IOptions<AuthorizationOptions> options)
        {
            MyPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
        }
        public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => MyPolicyProvider.GetDefaultPolicyAsync();
        public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            if (policyName.StartsWith("Permission", StringComparison.OrdinalIgnoreCase))
            {
                var policy = new AuthorizationPolicyBuilder();
                policy.AddRequirements(new PermissionRequirement(policyName));
                return Task.FromResult(policy.Build());
            }
            return MyPolicyProvider.GetPolicyAsync(policyName);
        }
        public Task<AuthorizationPolicy> GetFallbackPolicyAsync() => MyPolicyProvider.GetFallbackPolicyAsync();//MyPolicyProvider.GetDefaultPolicyAsync();
    }
}
