namespace PackageService.Models
{
    public enum PackageState
    {
        New,
        InNetwork,
        WithDriver,
        FailedDelivery,
        Delivered
    }
}
