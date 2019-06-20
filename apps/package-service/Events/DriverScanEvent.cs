using PackageService.Models;

namespace PackageService.Events
{
    public class DriverScanEvent
    {
        public string Barcode { get; set; }
        public Driver Driver { get; set; }
    }
}
