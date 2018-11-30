using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Products.Api.Authorization.Requirements;
using Products.Api.Models;

namespace Products.Api.Authorization.Handlers
{
    public class OwnedProductHandler : AuthorizationHandler<OwnedProductRequirement, Product>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            OwnedProductRequirement requirement,
            Product resource)
        {
            var nameIdentifierClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);

            if (nameIdentifierClaim == null)
            {
                return Task.CompletedTask;
            }

            if (resource.OwnerId == nameIdentifierClaim.Value)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}