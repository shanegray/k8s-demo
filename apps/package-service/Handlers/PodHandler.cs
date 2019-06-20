using PackageService.Events;
using PackageService.Service;
using ShareMe.Carefully.Rabbit;
using System.Threading.Tasks;

namespace PackageService.Handlers
{
    public class PodHandler : GenericListener<PackageDeliveredEvent>
    {
        private readonly ReadModelService readModelService;

        public PodHandler(ReadModelService readModelService)
            : base("package.driver-scan.pod.service")
        {
            this.readModelService = readModelService;
        }

        public override Task<bool> HandleMessage(PackageDeliveredEvent message)
        {
            return this.readModelService.Delivered(message);
        }
    }
}
