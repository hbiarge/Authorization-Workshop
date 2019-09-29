using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Products.Api.Authorization.Requirements;

namespace Products.Api.Authorization.Handlers
{
    public class Handler01 : AuthorizationHandler<LastHandlerWinRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            LastHandlerWinRequirement requirement)
        {
            context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }

    public class Handler02 : AuthorizationHandler<LastHandlerWinRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            LastHandlerWinRequirement requirement)
        {
            context.Fail();

            return Task.CompletedTask;
        }
    }

    public class Handler03 : AuthorizationHandler<LastHandlerWinRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            LastHandlerWinRequirement requirement)
        {
            context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}