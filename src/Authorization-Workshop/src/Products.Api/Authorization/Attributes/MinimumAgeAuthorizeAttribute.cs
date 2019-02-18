using Microsoft.AspNetCore.Authorization;
using Products.Api.Authorization.Providers;

namespace Products.Api.Authorization.Attributes
{
    // This attribute derives from the [Authorize] attribute, adding 
    // the ability for a user to specify an 'age' parameter. Since authorization
    // policies are looked up from the policy provider only by string, this
    // authorization attribute creates is policy name based on a constant prefix
    // and the user-supplied age parameter. A custom authorization policy provider
    // (`MinimumAgePolicyProvider`) can then produce an authorization policy with 
    // the necessary requirements based on this policy name.
    internal class MinimumAgeAuthorizeAttribute : AuthorizeAttribute
    {
        public MinimumAgeAuthorizeAttribute(int age) => Age = age;

        // Get or set the Age property by manipulating the underlying Policy property
        public int Age
        {
            get
            {
                if (int.TryParse(Policy.Substring(MinimumAgePolicyProvider.PolicyPrefix.Length), out var age))
                {
                    return age;
                }
                return default(int);
            }
            set => Policy = $"{MinimumAgePolicyProvider.PolicyPrefix}{value.ToString()}";
        }
    }
}
