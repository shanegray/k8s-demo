using PackageService.Events;
using PackageService.Service;
using ShareMe.Carefully.Rabbit;
using System.Threading.Tasks;

namespace PackageService.Handlers
{
    public class VipHandler : GenericListener<DriverScanEvent>
    {
        private readonly ReadModelService readModelService;

        public VipHandler(ReadModelService readModelService)
            : base("package.driver-scan.vip.service")
        {
            this.readModelService = readModelService;
        }

        public override Task<bool> HandleMessage(DriverScanEvent message)
        {
            return this.readModelService.DriverScan(message);
        }
    }
}
