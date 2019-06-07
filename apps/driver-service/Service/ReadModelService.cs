using DriverService.Config;
using DriverService.Events;
using DriverService.Models;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace DriverService.Service
{
    public class ReadModelService
    {
        private readonly IMongoCollection<Driver> collection;

        public ReadModelService(MongoSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            this.collection = database.GetCollection<Driver>(settings.ReadModelCollectionName);
        }

        public async Task AddNewDriver(string driverId, string driverName)
        {
            var data = new Driver
            {
                DriverId = driverId,
                Name = driverName,
                Status = DriverStatus.Idle
            };

            await this.collection.InsertOneAsync(data);
        }

        public async Task<bool> UpdateStatus(string driverId, DriverStatus status)
        {
            var update = Builders<Driver>.Update.Set(t => t.Status, status);
            var result = await this.collection.UpdateOneAsync(t => t.DriverId == driverId, update);

            return result.ModifiedCount == 1;
        }

        public async Task<bool> UpdateDriverWithStats(string driverId, StatsUpdateEvent todaysStats, StatsUpdateEvent overallStats)
        {
            // get driver from DB (to update)
            var query = Builders<Driver>.Filter.Eq(t => t.DriverId, driverId);
            var driver = await this.collection.Find(query).FirstAsync();

            // Update the stats on the driver object
            driver.OverallStats = overallStats;
            driver.LatestStats.Add(todaysStats);

            // keep only the last 7 stats in the list
            if (driver.LatestStats.Count > 7)
            {
                driver.LatestStats.RemoveAt(0);
            }

            // replace the document
            // TODO : This should technically be an update statement for concurrency
            var result = await this.collection.ReplaceOneAsync(t => t.Id == driver.Id, driver, new UpdateOptions { IsUpsert = false });

            return result.ModifiedCount == 1;
        }
    }
}
