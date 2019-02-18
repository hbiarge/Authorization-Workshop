using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Products.Api.Controllers
{
    [Authorize]
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
