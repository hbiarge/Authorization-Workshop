using Microsoft.AspNetCore.Mvc;
using Products.Api.Authorization.Attributes;

namespace Products.Api.Controllers
{
    [Route("api/custom-provider")]
    public class S04CustomPolicyProviderController : ControllerBase
    {
        [HasPermission(Permission.Read)]
        [HttpGet("Read")]
        public IActionResult Read()
        {
            return Ok(new[] { "Value1", "Value2" });
        }

        [HasPermission(Permission.Write)]
        [HttpGet("Write")]
        public IActionResult Write()
        {
            return Ok(new[] { "Value1", "Value2" });
        }
    }
}