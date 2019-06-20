using MongoDB.Bson;
using MongoDB.Driver;
using PackageService.Events;
using PackageService.Models;
using PackageService.Settings;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PackageService.Service
{
    public class ReadModelService
    {
        private readonly IMongoCollection<Package> collection;

        public ReadModelService(MongoSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            this.collection = database.GetCollection<Package>(settings.ReadModelCollectionName);
        }

        public Task AddPackageToNetwork(AddedToNetworkEvent addedToNetworkEvent)
        {
            var package = new Package
            {
                Barcode = addedToNetworkEvent.Barcode,
                Addresses = new List<Address> { addedToNetworkEvent.Address },
                Customer = addedToNetworkEvent.Customer,
                State = PackageState.New
            };

            return this.collection.InsertOneAsync(package);
        }

        public async Task<bool> NetworkScan(NetworkScanEvent networkScanEvent, string scanType)
        {
            // NOTE : can do Find and Update/Replace - but this is better for concurrency (when applied with a model)
            // See comparison with DriverService.Service.ReadModelService.UpdateDriverWithStats

            // Below is like:
            // UPDATE Package
            // SET [State] = @State
            // ,   [Location] = @Location
            // WHERE Barcode = @Barcode

            var update = Builders<Package>.Update
                            .Set(t => t.State, PackageState.InNetwork)
                            // Because Location is "schema-less" we can set it to the following object: { depot: "<depot>" }
                            .Set(t => t.Location, new BsonDocument("depot", networkScanEvent.Depot));
            
            // Update based off matching barcode with update statement above
            var result = await this.collection.UpdateOneAsync(x => x.Barcode == networkScanEvent.Barcode, update);
            
            return result.ModifiedCount == 1;
        }

        public async Task<bool> DriverScan(DriverScanEvent driverScanEvent)
        {
            // Notes on this are similar to NetworkScan()
            var update = Builders<Package>.Update
                            .Set(t => t.State, PackageState.WithDriver)
                            // set it to the following object: { driver: { ... } }
                            .Set(t => t.Location, driverScanEvent.Driver.ToBsonDocument());

            // Could DRY this statement, easier to read for demo
            var result = await this.collection.UpdateOneAsync(x => x.Barcode == driverScanEvent.Barcode, update);

            return result.ModifiedCount == 1;
        }

        public async Task<bool> FailedDelivery(PackageFailedDeliveryEvent failedDeliveryEvent)
        {
            // Notes on this are similar to NetworkScan()
            var update = Builders<Package>.Update
                            .Set(t => t.State, PackageState.FailedDelivery)                            
                            .Set(t => t.Notes, new BsonDocument("failureReason", failedDeliveryEvent.FailedReason));

            // Could DRY this statement, easier to read for demo
            var result = await this.collection.UpdateOneAsync(x => x.Barcode == failedDeliveryEvent.Barcode, update);

            return result.ModifiedCount == 1;
        }

        public async Task<bool> Delivered(PackageDeliveredEvent packageDeliveredEvent)
        {
            // Notes on this are similar to NetworkScan()
            var update = Builders<Package>.Update
                            .Set(t => t.State, PackageState.Delivered)
                            .Set(t => t.Signature, packageDeliveredEvent.Signature)
                            .Set(t => t.Location, packageDeliveredEvent.LatLon.ToBsonDocument())
                            .Set(t => t.Notes, new BsonDocument("message", "signed for by customer"));

            // I Am copying and Pasting, I shouldn't - ¯\_(ツ)_/¯
            var result = await this.collection.UpdateOneAsync(x => x.Barcode == packageDeliveredEvent.Barcode, update);

            return result.ModifiedCount == 1;
        }
    }
}
