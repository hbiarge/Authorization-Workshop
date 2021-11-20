using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Products.Api.Scenarios.S04MultipleHandlersBehavior;

public static class Policies
{
    public const string HandlersBehavior = "HandlersBehavior";

    public static void AddPolicies(AuthorizationOptions options)
    {
        options.InvokeHandlersAfterFailure = true; // Defaults to true

        // Show how several succeed are handled
        options.AddPolicy(HandlersBehavior, policyBuilder =>
        {
            policyBuilder.AddRequirements(new LastHandlerWinRequirement());
        });
    }

    public static IServiceCollection AddHandlers(this IServiceCollection services)
    {
        services.AddSingleton<IAuthorizationHandler, Handler01>();
        services.AddSingleton<IAuthorizationHandler, Handler02>();
        services.AddSingleton<IAuthorizationHandler, Handler03>();

        return services;
    }
}

[Route("api/intro")]
public class Controller : ControllerBase
{
    [HttpGet("success")]
    [Authorize(Policies.HandlersBehavior)]
    public IActionResult HandlersBehavior()
    {
        return Ok(new[] { "Value1", "Value2" });
    }
}

public class LastHandlerWinRequirement : IAuthorizationRequirement
{
}

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
