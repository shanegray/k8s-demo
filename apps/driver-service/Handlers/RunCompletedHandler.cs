using DriverService.Events;
using DriverService.Models;
using DriverService.Service;
using ShareMe.Carefully.Rabbit;
using System.Threading.Tasks;

namespace DriverService.Handlers
{
    public class RunCompletedHandler : GenericListener<TimedEvent>
    {
        private readonly ReadModelService readModelService;

        public RunCompletedHandler(ReadModelService readModelService) : base("driver.status-update.run-completed.service")
        {
            this.readModelService = readModelService;
        }

        public override async Task<bool> HandleMessage(TimedEvent data)
        {
            // Update Status to Idle
            var success = await this.readModelService.UpdateStatus(data.DriverId, DriverStatus.Idle);
            //if (success == false) // driver not found
            // TODO : 

            // TODO : Get stats for today from Package API
            // TODO : Remove Mocks
            var todaysStats = new StatsUpdateEvent
            {
                PODs = 0.85m,
                DNCs = 0.15m
            };

            // TODO : Calculate overall stats from Package API
            // TODO : Remove Mocks
            var overAllStats = new StatsUpdateEvent
            {
                PODs = 0.864m,
                DNCs = 0.136m
            };

            return await this.readModelService.UpdateDriverWithStats(data.DriverId, todaysStats, overAllStats);
        }
    }
}
