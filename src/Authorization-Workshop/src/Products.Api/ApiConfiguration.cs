using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Products.Api.Authorization;
using Products.Api.Authorization.Handlers;
using Products.Api.Repositories;

namespace Products.Api
{
    public static class ApiConfiguration
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddAuthorization(Policies.Configure)
                .AddJsonFormatters(options =>
                {
                    options.NullValueHandling = NullValueHandling.Ignore;
                });

            // Repositories
            services.AddSingleton<IProductsRepository, InMemoryProductsRepository>();

            // Authorization handlers
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
