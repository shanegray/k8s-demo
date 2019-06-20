using PackageService.Events;
using PackageService.Service;
using ShareMe.Carefully.Rabbit;
using System.Threading.Tasks;

namespace PackageService.Handlers
{
    public class VopHandler : GenericListener<DriverScanEvent>
    {
        private readonly ReadModelService readModelService;

        public VopHandler(ReadModelService readModelService)
            : base("package.driver-scan.vop.service")
        {
            this.readModelService = readModelService;
        }

        public override async Task<bool> HandleMessage(DriverScanEvent message)
        {
            await this.readModelService.DriverScan(message);

            return true;
        }
    }
}
