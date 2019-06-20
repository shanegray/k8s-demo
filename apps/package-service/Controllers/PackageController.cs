using Microsoft.AspNetCore.Mvc;
using PackageService.Events;
using PackageService.Service;
using System.Threading.Tasks;

namespace PackageService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackageController : ControllerBase
    {
        private readonly EventStoreService eventStoreService;
        private readonly MessengerService messengerService;

        public PackageController(EventStoreService eventStoreService, MessengerService messengerService)
        {
            this.eventStoreService = eventStoreService;
            this.messengerService = messengerService;
        }

        [HttpGet]
        public IActionResult OkMessage()
        {
            return this.Ok(new { message = "OK" });
        }

        // Manifest
        [HttpPost]
        public async Task<IActionResult> ManifestPackage(AddedToNetworkEvent addedToNetworkEvent)
        {
            await this.eventStoreService.AddPackage(addedToNetworkEvent);
            this.messengerService.PackageAdded(addedToNetworkEvent);
            return this.Ok();
        }

        // HIP|DIP (refactor to one method - both network scan events)
        [HttpPost("{barcode}/hip")]
        public async Task<IActionResult> HIP(NetworkScanEvent networkScanEvent)
        {
            await this.eventStoreService.NetworkScan(networkScanEvent, PackageEventType.HubInbound);
            this.messengerService.WarehouseScan(networkScanEvent, "hip");
            return this.Ok();
        }

        [HttpPost("{barcode}/dip")]
        public async Task<IActionResult> DIP(NetworkScanEvent networkScanEvent)
        {
            await this.eventStoreService.NetworkScan(networkScanEvent, PackageEventType.DepotInbound);
            this.messengerService.WarehouseScan(networkScanEvent, "dip");
            return this.Ok();
        }

        // VIP|VOP (refactor to one method - both driver scan events)
        [HttpPost("{barcode}/vip")]
        public async Task<IActionResult> VIP(DriverScanEvent driverScanEvent)
        {
            await this.eventStoreService.DriverScan(driverScanEvent, PackageEventType.VanInbound);
            this.messengerService.DriverScan(driverScanEvent, "vip");
            return this.Ok();
        }

        // VOP   
        [HttpPost("{barcode}/vop")]
        public async Task<IActionResult> VOP(DriverScanEvent driverScanEvent)
        {
            await this.eventStoreService.DriverScan(driverScanEvent, PackageEventType.OnVan);
            this.messengerService.DriverScan(driverScanEvent, "vop");
            return this.Ok();
        }

        // DNC
        [HttpPost("{barcode}/dnc")]
        public async Task<IActionResult> DNC(PackageFailedDeliveryEvent packageFailedDeliveryEvent)
        {
            await this.eventStoreService.Failed(packageFailedDeliveryEvent);
            this.messengerService.FailedDelivery(packageFailedDeliveryEvent);
            return this.Ok();
        }

        // POD
        [HttpPost("{barcode}/pod")]
        public async Task<IActionResult> POD(PackageDeliveredEvent packageDeliveredEvent)
        {
            await this.eventStoreService.Delivered(packageDeliveredEvent);
            this.messengerService.Delivered(packageDeliveredEvent);
            return this.Ok();
        }
    }
}
