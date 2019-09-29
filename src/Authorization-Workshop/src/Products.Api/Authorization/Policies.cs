using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Products.Api.Authorization.Requirements;

namespace Products.Api.Authorization
{
    public static class Policies
    {
        public const string Default = "Default";
        public const string Simple = "Simple";
        public const string HandlersBehavior = "HandlersBehavior";
        public const string Over21Years = "Over21Years";
        public const string AllowedInOffice = "AllowedInOffice";
        public const string MultipleSchemas = "MultipleSchemas";

        public static void Configure(AuthorizationOptions options)
        {
            options.InvokeHandlersAfterFailure = true;

            // This is the default policy (created and used by default)
            // Can be replaced
            // options.DefaultPolicy = new AuthorizationPolicyBuilder().RequireRole("Admin").Build();
            options.AddPolicy(Default, builder =>
            {
                builder.RequireAuthenticatedUser();
            });

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

            // Policy with simple rules. Show how several succeed are handled
            options.AddPolicy(HandlersBehavior, policyBuilder =>
            {
                policyBuilder.AddRequirements(new LastHandlerWinRequirement());
            });

            // Code based policy: Calculations over Claims information
            options.AddPolicy(Over21Years, policyBuilder =>
            {
                policyBuilder.AddRequirements(new MinimumAgeRequirement(21));
            });

            // Code based policy: Several handlers to validate a requirement
            options.AddPolicy(AllowedInOffice, policyBuilder =>
            {
                policyBuilder.AddRequirements(new OfficeEntryRequirement(department: "CPM"));
            });

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
}
