using PackageService.Events;
using ShareMe.Carefully.Rabbit;

namespace PackageService.Service
{
    public class MessengerService
    {
        private readonly RabbitMessenger rabbitMessenger;

        public MessengerService(RabbitMessenger rabbitMessenger)
        {
            this.rabbitMessenger = rabbitMessenger;
        }

        public void PackageAdded(AddedToNetworkEvent addedToNetworkEvent)
        {
            this.rabbitMessenger.SendMessage("package.added", addedToNetworkEvent);
        }

        public void WarehouseScan(NetworkScanEvent networkScanEvent, string scan)
        {
            this.rabbitMessenger.SendMessage("package.warehouse-scan", networkScanEvent, scan);
        }

        public void DriverScan(DriverScanEvent driverScanEvent, string scan)
        {
            this.rabbitMessenger.SendMessage("package.driver-scan", driverScanEvent, scan);
        }

        public void FailedDelivery(PackageFailedDeliveryEvent packageFailedDeliveryEvent)
        {
            this.rabbitMessenger.SendMessage("package.driver-scan", packageFailedDeliveryEvent, "dnc");
        }

        public void Delivered(PackageDeliveredEvent packageDeliveredEvent)
        {
            this.rabbitMessenger.SendMessage("package.driver-scan", packageDeliveredEvent, "pod");
        }
    }
}
