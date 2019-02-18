using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Products.Api.Authorization.Requirements;

namespace Products.Api.Authorization.Handlers
{
    public class HasBadgeHandler : AuthorizationHandler<OfficeEntryRequirement>
    {
        private const string ExpectedIssuer = "MySuperSecureIssuer";

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OfficeEntryRequirement requirement)
        {
            var badgeClaim = context.User.FindFirst(c => c.Type == "BadgeNumber");
            var departmentClaim = context.User.FindFirst(c => c.Type == "Department");

            // Both claims should exist
            if (badgeClaim == null || departmentClaim == null)
            {
                return Task.CompletedTask; 
            }

            // The issuer of the badgeNumber claim should be known
            if (badgeClaim.Issuer != ExpectedIssuer)
            {
                return Task.CompletedTask;
            }

            // The department should match the requirement department
            if (departmentClaim.Value != requirement.Department)
            {
                return Task.CompletedTask;
            }

            context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
