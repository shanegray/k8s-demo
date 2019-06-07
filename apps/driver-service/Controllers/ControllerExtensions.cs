using Microsoft.AspNetCore.Mvc;

namespace DriverService.Controllers
{
    public static class ControllerExtensions
    {
        public static IActionResult OkMessage(this ControllerBase controller, string message = "OK")
        {
            return controller.Ok(new { message });
        }
    }
}
