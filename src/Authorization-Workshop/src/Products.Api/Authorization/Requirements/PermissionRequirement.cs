using Microsoft.AspNetCore.Authorization;
using Products.Api.Authorization.Attributes;

namespace Products.Api.Authorization.Requirements
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public PermissionRequirement(Permission permission)
        {
            Permission = permission;
        }

        public Permission Permission { get; }
    }
}