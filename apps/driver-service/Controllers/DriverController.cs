using DriverService.Events;
using DriverService.Service;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DriverService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriverController : ControllerBase
    {
        private readonly EventStoreService eventStoreService;
        private readonly ReadModelService readModelService;

        public DriverController(EventStoreService eventStoreService, ReadModelService readModelService)
        {
            this.eventStoreService = eventStoreService;
            this.readModelService = readModelService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return this.OkMessage();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] HiredEvent data)
        {
            await this.eventStoreService.Hire(data);
            return this.OkMessage();
        }

        [HttpPost("{driverId}/load-van")]
        public async Task<IActionResult> LoadVan([FromBody] TimedEvent data, string driverId)
        {
            await this.eventStoreService.LoadVan(driverId, data);
            return this.OkMessage();
        }        
    }
}
