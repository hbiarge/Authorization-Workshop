using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Products.Api.Authorization;

namespace Products.Api.Controllers
{
    [Route("api/intro")]
    public class S01BisIntroController : ControllerBase
    {
        [HttpGet("success")]
        [Authorize(Policies.HandlersBehavior)]
        public IActionResult HandlersBehavior()
        {
            return Ok(new[] { "Value1", "Value2" });
        }
    }
}