using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Products.Api.Authorization;
using Products.Api.Authorization.Handlers;
using Products.Api.Authorization.Providers;
using Products.Api.Repositories;
using Products.Api.Services;

namespace Products.Api
{
    public static class ApiConfiguration
    {
        public static void ConfigureServices(IMvcBuilder mvcBuilder)
        {
            mvcBuilder.AddApplicationPart(typeof(ApiConfiguration).Assembly);

            var services = mvcBuilder.Services;

            services.AddAuthorization(Policies.Configure);

            // Repositories
            services.AddSingleton<IProductsRepository, InMemoryProductsRepository>();

            // Services
            services.AddScoped<IScopedService, ScopedService>();

            // Replace the default authorization policy provider with our own
            // custom provider which can return authorization policies for given
            // policy names (instead of using the default policy provider)
            services.AddSingleton<IAuthorizationPolicyProvider, PermissionsPolicyProvider>();

            // Authorization handlers
           services.AddSingleton<IAuthorizationHandler, Handler01>();
           services.AddSingleton<IAuthorizationHandler, Handler02>();
           services.AddSingleton<IAuthorizationHandler, Handler03>();
           services.AddSingleton<IAuthorizationHandler, MinimumAgeAuthorizationHandler>();
           services.AddSingleton<IAuthorizationHandler, HasBadgeHandler>();
           services.AddSingleton<IAuthorizationHandler, HasTemporaryPassHandler>();
           services.AddSingleton<IAuthorizationHandler, OwnedProductHandler>();
           services.AddSingleton<IAuthorizationHandler, PermissionsAuthorizationHandler>();
        }
    }
}
