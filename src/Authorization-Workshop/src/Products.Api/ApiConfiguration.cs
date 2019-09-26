using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Products.Api.Authorization;
using Products.Api.Authorization.Handlers;
using Products.Api.Authorization.Providers;
using Products.Api.Repositories;
using Products.Api.Services;

namespace Products.Api
{
    public static class ApiConfiguration
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddAuthorization(Policies.Configure)
                .AddJsonFormatters(options =>
                {
                    options.NullValueHandling = NullValueHandling.Ignore;
                });

            // Repositories
            services.AddSingleton<IProductsRepository, InMemoryProductsRepository>();

            // Services
            services.AddScoped<IScopedService, ScopedService>();

            // Replace the default authorization policy provider with our own
            // custom provider which can return authorization policies for given
            // policy names (instead of using the default policy provider)
            services.AddSingleton<IAuthorizationPolicyProvider, PermissionsPolicyProvider>();

            // Authorization handlers
            services.AddSingleton<IAuthorizationHandler, MinimumAgeAuthorizationHandler>();
            services.AddSingleton<IAuthorizationHandler, PermissionsAuthorizationHandler>();
            services.AddSingleton<IAuthorizationHandler, HasBadgeHandler>();
            services.AddSingleton<IAuthorizationHandler, HasTemporaryPassHandler>();
            services.AddSingleton<IAuthorizationHandler, OwnedProductHandler>();
        }

        public static void Configure(IApplicationBuilder app)
        {
            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
