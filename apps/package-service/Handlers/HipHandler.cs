using PackageService.Events;
using PackageService.Service;
using ShareMe.Carefully.Rabbit;
using System.Threading.Tasks;

namespace PackageService.Handlers
{
    public class HipHandler : GenericListener<NetworkScanEvent>
    {
        private readonly ReadModelService readModelService;

        public HipHandler(ReadModelService readModelService)
            : base("package.warehouse-scan.hip.service")
        {
            this.readModelService = readModelService;
        }

        public override Task<bool> HandleMessage(NetworkScanEvent message)
        {
            return this.readModelService.NetworkScan(message, "HIP");
        }
    }
}
