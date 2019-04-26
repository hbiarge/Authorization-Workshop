using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Products.Api.Authorization.Requirements;
using Products.Api.Repositories;

namespace Products.Api.Controllers
{
    [Authorize]
    [Route("api/demos/products")]
    public class S03ResourceBasedController : ControllerBase
    {
        private readonly IProductsRepository _productsRepository;
        private readonly IAuthorizationService _authorizationService;

        public S03ResourceBasedController(
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

            if (product == null)
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
}