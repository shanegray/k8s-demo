using PackageService.Models;

namespace PackageService.Events
{
    public class AddedToNetworkEvent
    {
        public string Barcode { get; set; }
        public Address Address { get; set; }
        public Person Customer { get; set; }
    }
}
