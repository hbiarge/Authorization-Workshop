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
        public const string Over21Years = "Over21Years";
        public const string AllowedInOffice = "AllowedInOffice";
        public const string MultipleSchemas = "MultipleSchemas";

        public static void Configure(AuthorizationOptions options)
        {
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

            // Code based policy: Calculations over Claims information
            options.AddPolicy(Over21Years, builder =>
            {
                builder.AddRequirements(new MinimumAgeRequirement(21));
            });

            // Code based policy: Several handlers to validate a requirement
            options.AddPolicy(AllowedInOffice, builder =>
            {
                builder.AddRequirements(new OfficeEntryRequirement(department: "CPM"));
            });

            // Use of different schemas
            options.AddPolicy(MultipleSchemas, builder =>
            {
                builder.AddAuthenticationSchemes("bearer", "extra");
                builder.RequireAuthenticatedUser();
                builder.RequireRole("Operator"); // From bearer scheme
                builder.RequireClaim("Supervisor"); // From extra scheme
            });
        }
    }
}
