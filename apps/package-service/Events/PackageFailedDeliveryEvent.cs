namespace PackageService.Events
{
    public class PackageFailedDeliveryEvent
    {
        public string Barcode { get; set; }
        public string FailedReason { get; set; }
    }
}
