using Microsoft.AspNetCore.Mvc;

namespace PackageService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackageController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return this.Ok(new string[] { "one", "two" });
        }
    }
}
