using Microsoft.AspNetCore.Mvc;
using Products.Api.Authorization.Attributes;

namespace Products.Api.Controllers
{
    [Route("api/custom-provider")]
    public class S04CustomPolicyProviderController : ControllerBase
    {
        [MinimumAgeAuthorize(50)]
        [HttpGet("50")]
        public IActionResult Minimum50()
        {
            return Ok(new[] { "Value1", "Value2" });
        }

        [MinimumAgeAuthorize(40)]
        [HttpGet("40")]
        public IActionResult Minimum40()
        {
            return Ok(new[] { "Value1", "Value2" });
        }
    }
}