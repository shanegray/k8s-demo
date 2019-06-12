using DriverService.Events;
using DriverService.Service;
using ShareMe.Carefully.Rabbit;
using System.Threading.Tasks;

namespace DriverService.Handlers
{
    public class DriverHiredHandler : GenericListener<HiredEvent>
    {
        private readonly ReadModelService readModelService;

        public DriverHiredHandler(ReadModelService readModelService) :
            base("driver.hired.service")
        {
            this.readModelService = readModelService;
        }

        public override async Task<bool> HandleMessage(HiredEvent data)
        {
            await this.readModelService.AddNewDriver(data.DriverId, data.Name);

            return true;
        }
    }
}
