using System;
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
    //[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false)]
    //internal class MinimumAgeAuthorizeAttribute : AuthorizeAttribute
    //{
    //    public MinimumAgeAuthorizeAttribute(int age) => Age = age;

    //    // Get or set the Permission property by manipulating the underlying Policy property
    //    public int Age
    //    {
    //        get
    //        {
    //            if (int.TryParse(Policy.Substring(MinimumAgePolicyProvider.PolicyPrefix.Length), out var age))
    //            {
    //                return age;
    //            }
    //            return default(int);
    //        }
    //        set => Policy = $"{MinimumAgePolicyProvider.PolicyPrefix}{value.ToString()}";
    //    }
    //}

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false)]
    internal class HasPermissionAttribute : AuthorizeAttribute
    {
        public HasPermissionAttribute(Permission permission) => Permission = permission;

        // Get or set the Permission property by manipulating the underlying Policy property
        public Permission Permission
        {
            get
            {
                if (Enum.TryParse<Permission>(Policy.Substring(PermissionsPolicyProvider.PolicyPrefix.Length), out var permission))
                {
                    return permission;
                }
                return Permission.None;
            }
            set => Policy = $"{PermissionsPolicyProvider.PolicyPrefix}{value.ToString()}";
        }
    }

    public enum Permission
    {
        None = 0,
        Read,
        Write
    }
}
