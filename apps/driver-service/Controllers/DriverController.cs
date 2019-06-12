using DriverService.Events;
using DriverService.Models;
using DriverService.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace DriverService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriverController : ControllerBase
    {
        private readonly EventStoreService eventStoreService;
        private readonly ReadModelService readModelService;
        private readonly MessengerService messengerService;

        public DriverController(
            EventStoreService eventStoreService, 
            ReadModelService readModelService,
            MessengerService messengerService)
        {
            this.eventStoreService = eventStoreService;
            this.readModelService = readModelService;
            this.messengerService = messengerService;
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
            this.messengerService.DriverHiredMessage(data);
            return this.OkMessage();
        }


        // api/driver/<driver-id>/load-van
        // api/driver/<driver-id>/run-started
        // api/driver/<driver-id>/run-complete
        [HttpPost("{driverId}/{eventString}")]
        public async Task<IActionResult> UpdateStatus(string driverId, string eventString)
        {
            var eventType = GetEventTypeFromUrlSegment(eventString);

            if (eventType == null)
                return this.NotFound();

            var timedEvent = new TimedEvent
            {
                DriverId = driverId,
                Time = DateTime.UtcNow
            };

            await this.eventStoreService.AddStatusEvent(driverId, eventType.Value, timedEvent);
            this.messengerService.StatusUpdate(timedEvent, eventString);

            return this.OkMessage();
        }

        private DriverEventType? GetEventTypeFromUrlSegment(string segment)
        {
            switch (segment)
            {
                case "load-van":
                    return DriverEventType.LoadingVan;
                case "run-started":
                    return DriverEventType.RunStarted;
                case "run-complete":
                    return DriverEventType.RunComplete;
                default:
                    return null;
            }
        }
    }
}
