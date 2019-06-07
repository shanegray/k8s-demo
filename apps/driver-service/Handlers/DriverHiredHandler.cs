using DriverService.Events;
using DriverService.Service;
using System.Threading.Tasks;

namespace DriverService.Handlers
{
    public class DriverHiredHandler
    {
        private readonly ReadModelService readModelService;

        public DriverHiredHandler(ReadModelService readModelService)
        {
            this.readModelService = readModelService;
        }

        public async Task HandleMessage(HiredEvent data)
        {
            await this.readModelService.AddNewDriver(data.DriverId, data.Name);
        }
    }
}
