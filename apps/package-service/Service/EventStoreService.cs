using MongoDB.Driver;
using PackageService.Events;
using PackageService.Models;
using PackageService.Settings;
using ShareMe.Carefully;
using System.Threading.Tasks;

namespace PackageService.Service
{
    public class EventStoreService
    {
        private readonly IMongoCollection<EventStore> collection;

        public EventStoreService(MongoSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            this.collection = database.GetCollection<EventStore>(settings.EventStoreCollectionName);
        }

        public async Task AddPackage(AddedToNetworkEvent addedToNetworkEvent)
        {
            await this.Insert(addedToNetworkEvent.Barcode, PackageEventType.AddedToNetwork, addedToNetworkEvent);
        }

        public async Task NetworkScan(NetworkScanEvent networkScanEvent, PackageEventType packageEventType)
        {
            await this.Insert(networkScanEvent.Barcode, packageEventType, networkScanEvent);
        }

        public async Task DriverScan(DriverScanEvent driverScanEvent, PackageEventType packageEventType)
        {
            await this.Insert(driverScanEvent.Barcode, packageEventType, driverScanEvent);
        }

        public async Task Failed(PackageFailedDeliveryEvent packageFailedDeliveryEvent)
        {
            await this.Insert(packageFailedDeliveryEvent.Barcode, PackageEventType.DeliveryFailed, packageFailedDeliveryEvent);
        }

        public async Task Delivered(PackageDeliveredEvent packageDelivered)
        {
            await this.Insert(packageDelivered.Barcode, PackageEventType.Delivered, packageDelivered);
        }

        private async Task Insert(string barcode, PackageEventType eventType, object data)
        {
            var evt = new EventStore
            {
                Barcode = barcode,
                Event = eventType,
                Data = new ObjectToBsonDocument(data).Convert()
            };

            await this.collection.InsertOneAsync(evt);
        }
    }
}
