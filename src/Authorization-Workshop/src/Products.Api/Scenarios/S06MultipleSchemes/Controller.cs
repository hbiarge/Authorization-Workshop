using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Products.Api.Scenarios.S06MultipleSchemes;

public static class Policies
{
    public const string MultipleSchemas = "MultipleSchemas";

    public static void AddPolicies(AuthorizationOptions options)
    {
        // Use of different schemas
        options.AddPolicy(MultipleSchemas, policyBuilder =>
        {
            policyBuilder.AddAuthenticationSchemes("bearer", "extra");
            policyBuilder.RequireAuthenticatedUser();
            policyBuilder.RequireRole("Operator"); // From bearer scheme
            policyBuilder.RequireClaim("Supervisor"); // From extra scheme
        });
    }
}

[Route("api/schemas")]
public class Controller : ControllerBase
{
    [Authorize(Policies.MultipleSchemas)]
    [HttpGet]
    public IActionResult Values()
    {
        return Ok(new[] { "Value1", "Value2" });
    }
}
