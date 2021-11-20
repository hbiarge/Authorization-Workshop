using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Products.Api.Scenarios.S05ResourceBasedPolicy;

public static class Policies
{
    public static IServiceCollection AddHandlers(this IServiceCollection services)
    {
        services.AddSingleton<IAuthorizationHandler, OwnedProductHandler>();
        services.AddSingleton<IProductsRepository, InMemoryProductsRepository>();

        return services;
    }
}

[Authorize]
[Route("api/demos/products")]
public class Controller : ControllerBase
{
    private readonly IProductsRepository _productsRepository;
    private readonly IAuthorizationService _authorizationService;

    public Controller(
        IProductsRepository productsRepository,
        IAuthorizationService authorizationService)
    {
        _productsRepository = productsRepository;
        _authorizationService = authorizationService;
    }

    [HttpGet("productNumber")]
    public async Task<IActionResult> GetProduct(string productNumber)
    {
        var product = _productsRepository.Get(productNumber);

        if (product is null)
        {
            return NotFound();
        }

        var result = await _authorizationService.AuthorizeAsync(User, product, new OwnedProductRequirement());

        if (result.Succeeded)
        {
            return Ok(product);
        }

        return Forbid();
    }
}

public class OwnedProductRequirement : IAuthorizationRequirement
{
}

public class OwnedProductHandler : AuthorizationHandler<OwnedProductRequirement, Product>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        OwnedProductRequirement requirement,
        Product resource)
    {
        var nameIdentifierClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);

        if (resource.OwnerId == nameIdentifierClaim?.Value)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}

public class Product
{
    public string ProductNumber { get; set; }
    public string OwnerId { get; set; }
    public string ProductName { get; set; }
}

public interface IProductsRepository
{
    Product Get(string productNumber);
}

public class InMemoryProductsRepository : IProductsRepository
{
    private readonly List<Product> _products = new()
    {
        new Product
        {
            ProductNumber = "1234",
            OwnerId = "HB04356",
            ProductName = "Clean code"
        },
        new Product
        {
            ProductNumber = "4321",
            OwnerId = "RG95478",
            ProductName = "Clean code"
        },
    };

    public Product Get(string productNumber)
    {
        return _products.FirstOrDefault(p => p.ProductNumber == productNumber);
    }
}