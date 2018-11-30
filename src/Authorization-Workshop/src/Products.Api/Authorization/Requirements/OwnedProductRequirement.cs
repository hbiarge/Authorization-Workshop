using Microsoft.AspNetCore.Authorization;

namespace Products.Api.Authorization.Requirements
{
    public class OwnedProductRequirement : IAuthorizationRequirement
    {
    }
}