using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Products.Api.Authorization;

namespace Products.Api.Controllers
{
    [Route("api/schemas")]
    public class S05SchemasController : ControllerBase
    {
        [Authorize(Policies.MultipleSchemas)]
        [HttpGet]
        public IActionResult Values()
        {
            return Ok(new[] { "Value1", "Value2" });
        }
    }
}
