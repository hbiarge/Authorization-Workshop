using Acheve.AspNetCore.TestHost.Security;
using Acheve.TestHost;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Products.Api.IntegrationTests;

public class TestStartup
{
    public void ConfigureServices(IServiceCollection services)
    {
        // Register Authentication
        services.AddAuthentication(options =>
            {
                options.DefaultScheme = TestServerDefaults.AuthenticationScheme;
            })
            .AddTestServer()
            // We can register as many TestServer authentication handlers with different scheme names
            .AddTestServer("bearer", options =>
            {
                options.NameClaimType = "name";
                options.RoleClaimType = "role";
            })
            .AddTestServer("extra");

        var mvcBuilder = services.AddControllers();

        ApiConfiguration.ConfigureServices(mvcBuilder);
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
