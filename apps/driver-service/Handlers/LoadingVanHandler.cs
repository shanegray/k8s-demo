using DriverService.Events;
using DriverService.Models;
using DriverService.Service;
using ShareMe.Carefully.Rabbit;
using System.Threading.Tasks;

namespace DriverService.Handlers
{
    public class LoadingVanHandler : GenericListener<TimedEvent>
    {
        private readonly ReadModelService readModelService;

        public LoadingVanHandler(ReadModelService readModelService) : base("driver.status-update.load-van.service")
        {            
            this.readModelService = readModelService;
        }

        public override Task<bool> HandleMessage(TimedEvent data)
        {            
            return this.readModelService.UpdateStatus(data.DriverId, DriverStatus.LoadingVan);
        }
    }
}
