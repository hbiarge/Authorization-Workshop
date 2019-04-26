using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Products.Api.Authorization.Requirements;
using Products.Api.Services;

namespace Products.Api.Authorization.Handlers
{
    public class MinimumAgeHandler : AuthorizationHandler<MinimumAgeRequirement>
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