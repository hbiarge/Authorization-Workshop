using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Products.Api.Authorization;

namespace Products.Api.Controllers
{
    [Authorize(Policies.Simple)]
    [Route("api/intro")]
    public class S01IntroController : ControllerBase
    {
        [HttpGet]
        public IActionResult Values()
        {
            return Ok(new[] { "Value1", "Value2" });
        }

        [AllowAnonymous]
        [HttpGet("public")]
        public IActionResult PublicValues()
        {
            return Ok(new[] { "Value1", "Value2" });
        }
    }
}
