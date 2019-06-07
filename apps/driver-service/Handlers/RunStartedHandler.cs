using DriverService.Events;
using DriverService.Models;
using DriverService.Service;
using System.Threading.Tasks;

namespace DriverService.Handlers
{
    public class RunStartedHandler
    {
        private readonly ReadModelService readModelService;

        public RunStartedHandler(ReadModelService readModelService)
        {
            this.readModelService = readModelService;
        }

        public async Task HandleMessage(TimedEvent data)
        {
            await this.readModelService.UpdateStatus(data.DriverId, DriverStatus.OnRun);

            // TODO : Success Reporting
        }
    }
}
