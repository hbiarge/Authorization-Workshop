using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Products.Api.Authorization.Requirements
{
    public class MinimumAgeRequirement : AuthorizationHandler<MinimumAgeRequirement>, IAuthorizationRequirement
    {
        private readonly int _minimumAge;

        public MinimumAgeRequirement(int minimumAge)
        {
            _minimumAge = minimumAge;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumAgeRequirement requirement)
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

            if (calculatedAge >= _minimumAge)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
