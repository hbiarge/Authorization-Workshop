using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Products.Api.Authorization.Attributes;
using Products.Api.Authorization.Requirements;

namespace Products.Api.Authorization.Handlers
{
    public class PermissionsAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            var claims = context.User
                .FindAll(c => c.Type == ClaimTypes.Role)
                .Select(x => x.Value)
                .ToArray();

            if (claims.Any() == false)
            {
                return Task.CompletedTask;
            }

            if (requirement.Permission == Permission.Read && claims.Contains("Developer"))
            {
                context.Succeed(requirement);
            }

            if (requirement.Permission == Permission.Write && claims.Contains("Master"))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}