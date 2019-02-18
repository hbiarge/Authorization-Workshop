using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Products.Api.Authorization.Requirements;

namespace Products.Api.Authorization.Handlers
{
    public class HasTemporaryPassHandler : AuthorizationHandler<OfficeEntryRequirement>
    {
        private const string ExpectedIssuer = "MySuperSecureIssuer";

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OfficeEntryRequirement requirement)
        {
            var temporaryBadgeClaim = context.User.FindFirst(c => c.Type == "TemporaryBadgeExpiry");
            
            if (temporaryBadgeClaim == null)
            {
                return Task.CompletedTask;
            }

            // The issuer of the TemporaryBadgeClaim claim should be known
            if (temporaryBadgeClaim.Issuer != ExpectedIssuer)
            {
                return Task.CompletedTask;
            }

            var temporaryBadgeExpiry = Convert.ToDateTime(temporaryBadgeClaim.Value).ToUniversalTime();

            if (temporaryBadgeExpiry > DateTime.UtcNow)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}