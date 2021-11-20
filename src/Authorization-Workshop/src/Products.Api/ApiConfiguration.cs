using Microsoft.Extensions.DependencyInjection;

namespace Products.Api;

public static class ApiConfiguration
{
    public static void ConfigureServices(IMvcBuilder mvcBuilder)
    {
        mvcBuilder.AddApplicationPart(typeof(ApiConfiguration).Assembly);

        var services = mvcBuilder.Services;

        services.AddAuthorization(options =>
        {
            Scenarios.S01ClaimsBasedPolicy.Policies.AddPolicies(options);
            Scenarios.S02CodeBasedPolicy.Policies.AddPolicies(options);
            Scenarios.S03MultipleHandlers.Policies.AddPolicies(options);
            Scenarios.S04MultipleHandlersBehavior.Policies.AddPolicies(options);
            Scenarios.S06MultipleSchemes.Policies.AddPolicies(options);
        });

        // Authorization handlers
        Scenarios.S02CodeBasedPolicy.Policies.AddHandlers(services);
        Scenarios.S03MultipleHandlers.Policies.AddHandlers(services);
        Scenarios.S05ResourceBasedPolicy.Policies.AddHandlers(services);
        Scenarios.S07CustomPolicyProvider.Policies.AddHandlers(services);
    }
}
