using PackageService.Events;
using PackageService.Service;
using ShareMe.Carefully.Rabbit;
using System.Threading.Tasks;

namespace PackageService.Handlers
{
    public class DipHandler : GenericListener<NetworkScanEvent>
    {
        private readonly ReadModelService readModelService;

        public DipHandler(ReadModelService readModelService)
            : base("package.warehouse-scan.dip.service")
        {
            this.readModelService = readModelService;
        }

        public override Task<bool> HandleMessage(NetworkScanEvent message)
        {
            return this.readModelService.NetworkScan(message, "DIP");
        }
    }
}
