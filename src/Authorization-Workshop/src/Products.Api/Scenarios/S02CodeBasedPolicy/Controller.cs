using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Products.Api.Scenarios.S02CodeBasedPolicy
{
    public static class Policies
    {
        public const string Over21Years = "Over21Years";

        public static void AddPolicies(AuthorizationOptions options)
        {
            // Code based policy: Calculations over Claims information
            options.AddPolicy(Over21Years, policyBuilder =>
            {
                policyBuilder.AddRequirements(new MinimumAgeRequirement(21));
            });
        }

        public static IServiceCollection AddHandlers(this IServiceCollection services)
        {
            services.AddSingleton<IAuthorizationHandler, MinimumAgeAuthorizationHandler>();

            return services;
        }
    }

    [Route("api/code-based")]
    public class Controller : ControllerBase
    {
        [Authorize(Policies.Over21Years)]
        [HttpGet("over-21")]
        public IActionResult OnlyForOldPeople()
        {
            return Ok(new[] { "Value1", "Value2" });
        }
    }

    public class MinimumAgeRequirement : IAuthorizationRequirement
    {
        public MinimumAgeRequirement(int minimumAge)
        {
            MinimumAge = minimumAge;
        }

        public int MinimumAge { get; }
    }

    public class MinimumAgeAuthorizationHandler : AuthorizationHandler<MinimumAgeRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            MinimumAgeRequirement requirement)
        {
            var claim = context.User.FindFirst(c => c.Type == ClaimTypes.DateOfBirth);

            if (claim == null)
            {
                return Task.CompletedTask;
            }

            var dateOfBirth = Convert.ToDateTime(claim.Value);

            int calculatedAge = DateTime.Today.Year - dateOfBirth.Year;

            if (dateOfBirth > DateTime.Today.AddYears(-calculatedAge))
            {
                calculatedAge--;
            }

            if (calculatedAge >= requirement.MinimumAge)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
