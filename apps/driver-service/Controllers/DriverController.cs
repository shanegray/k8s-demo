using Microsoft.AspNetCore.Mvc;

namespace DriverService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriverController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return this.Ok(new string[] { "one", "two" });
        }
    }
}
