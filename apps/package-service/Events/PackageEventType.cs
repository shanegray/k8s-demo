namespace PackageService.Events
{
    public enum PackageEventType
    {
        AddedToNetwork, // manifest
        HubInbound, // HIP
        DepotInbound, // DIP
        OnVan, // VOP
        DeliveryFailed, // DNC
        Delivered, // POD
        VanInbound // VIP
    }
}
