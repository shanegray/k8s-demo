using DriverService.Events;
using DriverService.Models;
using DriverService.Service;
using System.Threading.Tasks;

namespace DriverService.Handlers
{
    public class LoadingVanHandler
    {
        private readonly ReadModelService readModelService;

        public LoadingVanHandler(ReadModelService readModelService)
        {
            this.readModelService = readModelService;
        }

        public async Task HandleMessage(TimedEvent data)
        {
            var success = await this.readModelService.UpdateStatus(data.DriverId, DriverStatus.LoadingVan);

            // TODO : Success Reporting
        }
    }
}
