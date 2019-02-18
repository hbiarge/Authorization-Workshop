using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Products.Api.Authorization;

namespace Products.Api.Controllers
{
    [Route("api/code-based")]
    public class S02CodeBasedController : ControllerBase
    {
        [Authorize(Policies.Over21Years)]
        [HttpGet("over-21")]
        public IActionResult OnlyForOldPeople()
        {
            return Ok(new[] { "Value1", "Value2" });
        }

        [Authorize(Policies.AllowedInOffice)]
        [HttpGet("office")]
        public IActionResult SeveralOptionsToEnterTheOffice()
        {
            return Ok(new[] { "Value1", "Value2" });
        }
    }
}
