using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Products.Api.Authorization.Attributes;
using Products.Api.Authorization.Requirements;

namespace Products.Api.Authorization.Providers
{
    public class PermissionsPolicyProvider : IAuthorizationPolicyProvider
    {
        public const string PolicyPrefix = "Permission";

        private readonly IOptions<AuthorizationOptions> _options;

        public DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }

        public PermissionsPolicyProvider(IOptions<AuthorizationOptions> options)
        {
            _options = options;

            // ASP.NET Core only uses one authorization policy provider, so if the custom implementation
            // doesn't handle all policies (including default policies, etc.) it should fall back to an
            // alternate provider.
            //
            // In this sample, a default authorization policy provider (constructed with options from the 
            // dependency injection container) is used if this custom provider isn't able to handle a given
            // policy name.
            //
            // If a custom policy provider is able to handle all expected policy names then, of course, this
            // fallback pattern is unnecessary.
            FallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
        }

        public async Task<AuthorizationPolicy> GetDefaultPolicyAsync()
        {
            return await FallbackPolicyProvider.GetDefaultPolicyAsync();
        }

        // Policies are looked up by string name, so expect 'parameters' (like age)
        // to be embedded in the policy names. This is abstracted away from developers
        // by the more strongly-typed attributes derived from AuthorizeAttribute
        // (like [MinimumAgeAuthorize] in this sample)
        public async Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            // First try to get the policy from the existing policies
            var policy = await FallbackPolicyProvider.GetPolicyAsync(policyName);

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