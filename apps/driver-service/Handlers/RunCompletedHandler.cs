using DriverService.Events;
using DriverService.Models;
using DriverService.Service;
using System.Threading.Tasks;

namespace DriverService.Handlers
{
    public class RunCompletedHandler
    {
        private readonly ReadModelService readModelService;

        public RunCompletedHandler(ReadModelService readModelService)
        {
            this.readModelService = readModelService;
        }

        public async Task HandleMessage(TimedEvent data)
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

            await this.readModelService.UpdateDriverWithStats(data.DriverId, todaysStats, overAllStats);
        }
    }
}
