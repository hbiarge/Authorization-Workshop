using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Products.Api.Scenarios.S01ClaimsBasedPolicy
{
    public static class Policies
    {
        public const string Simple = "Simple";

        public static void AddPolicies(AuthorizationOptions options)
        {
            // Policy with simple rules. Based in User claims information
            options.AddPolicy(Simple, policyBuilder =>
            {
                policyBuilder.RequireRole("Developer");
                policyBuilder.RequireUserName("Hugo Biarge");
                policyBuilder.RequireClaim("Department", "CPM", "Platform");
                policyBuilder.RequireAssertion(context =>
                {
                    var roles = context.User.FindAll(ClaimTypes.Role);

                    return roles.Any()
                        ? Task.FromResult(true)
                        : Task.FromResult(false);
                });
            });
        }
    }

    [Authorize(Policies.Simple)]
    [Route("api/intro")]
    public class Controller : ControllerBase
    {
        [HttpGet]
        public IActionResult Values()
        {
            return Ok(new[] { "Value1", "Value2" });
        }

        [AllowAnonymous]
        [HttpGet("public")]
        public IActionResult PublicValues()
        {
            return Ok(new[] { "Value1", "Value2" });
        }
    }
}
