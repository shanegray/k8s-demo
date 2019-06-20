using PackageService.Events;
using PackageService.Service;
using ShareMe.Carefully.Rabbit;
using System.Threading.Tasks;

namespace PackageService.Handlers
{
    public class DncHandler : GenericListener<PackageFailedDeliveryEvent>
    {
        private readonly ReadModelService readModelService;

        public DncHandler(ReadModelService readModelService)
            : base("package.driver-scan.dnc.service")
        {
            this.readModelService = readModelService;
        }

        public override Task<bool> HandleMessage(PackageFailedDeliveryEvent message)
        {
            return this.readModelService.FailedDelivery(message);
        }
    }
}
