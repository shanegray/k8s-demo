using PackageService.Models;

namespace PackageService.Events
{
    public class PackageDeliveredEvent
    {
        public string Barcode { get; set; }
        public Signature Signature { get; set; }
        public LatLon LatLon { get; set; }
    }
}
