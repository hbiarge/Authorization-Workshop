using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Products.Api.Authorization.Attributes;
using Products.Api.Authorization.Requirements;

namespace Products.Api.Authorization.Providers
{
    public class PermissionsPolicyProvider : DefaultAuthorizationPolicyProvider
    {
        public const string PolicyPrefix = "Permission";

        private readonly IOptions<AuthorizationOptions> _options;
        
        public PermissionsPolicyProvider(IOptions<AuthorizationOptions> options)
            : base(options)
        {
            _options = options;
        }

        // Policies are looked up by string name, so expect 'parameters' (like age)
        // to be embedded in the policy names. This is abstracted away from developers
        // by the more strongly-typed attributes derived from AuthorizeAttribute
        // (like [MinimumAgeAuthorize] in this sample)
        public override async Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            // First try to get the policy from the existing policies
            var policy = await base.GetPolicyAsync(policyName);

            if (policy != null)
            {
                return policy;
            }

            // Then check if it´s a dynamic generated policy
            if (policyName.StartsWith(PolicyPrefix, StringComparison.OrdinalIgnoreCase)
                &&
                Enum.TryParse<Permission>(policyName.Substring(PolicyPrefix.Length), out var permission))
            {
                // Generate the policy
                var policyBuilder = new AuthorizationPolicyBuilder();
                policyBuilder.AddRequirements(new PermissionRequirement(permission));
                policy = policyBuilder.Build();

                // Add the policy to the known policies (cache)
                _options.Value.AddPolicy(policyName, policy);

                return policy;
            }

            // If the policy name doesn't match the format expected by this policy provider,
            // this would return null instead.
            return null;
        }
    }
}