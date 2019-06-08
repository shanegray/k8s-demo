using Bogus;
using DriverService.Config;
using DriverService.Events;
using DriverService.Models;
using DriverService.Service;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DriverService.Tests
{
    public class DriverTestFixture : IDisposable
    {
        public List<string> driversToDelete = new List<string>();
        public IMongoCollection<EventStore> EventsCollection { get; }
        public IMongoCollection<Driver> DriverCollection { get; }
        public EventStoreService EventStoreService { get; }
        public ReadModelService ReadModelService { get; }

        public DriverTestFixture()
        {
            // TODO : Pull from settings (can use a whole separate database just for testing)
            var mongoSettings = new MongoSettings
            {
                ConnectionString = "mongodb://localhost:27020",
                DatabaseName = "drivers",
                EventStoreCollectionName = "events",
                ReadModelCollectionName = "drivers"
            };

            var client = new MongoClient(mongoSettings.ConnectionString);
            var database = client.GetDatabase(mongoSettings.DatabaseName);

            this.EventsCollection = database.GetCollection<EventStore>(mongoSettings.EventStoreCollectionName);
            this.DriverCollection = database.GetCollection<Driver>(mongoSettings.ReadModelCollectionName);

            this.EventStoreService = new EventStoreService(mongoSettings);
            this.ReadModelService = new ReadModelService(mongoSettings);
        }

        public HiredEvent UniqueHireEvent()
        {
            var faker = new Faker();

            var evt = new HiredEvent
            { 
                DriverId = Guid.NewGuid().ToString(),
                Name = faker.Name.FullName()
            };

            this.driversToDelete.Add(evt.DriverId);

            return evt;
        }

        public async Task<string> CreateDriverHiredEvent()
        {
            var uniqueHire = UniqueHireEvent(); // will add id for clean up

            await this.EventStoreService.Hire(uniqueHire);

            return uniqueHire.DriverId;
        }

        public async Task<Driver> CreateDriverModel()
        {
            var uniqueHire = UniqueHireEvent();

            await this.ReadModelService.AddNewDriver(uniqueHire.DriverId, uniqueHire.Name);

            return new Driver { DriverId = uniqueHire.DriverId, Name = uniqueHire.Name };
        }

        public Task<List<EventStore>> GetDriverEventsFromDb(string id)
        {
            // sample linq for yas
            var query = from q in this.EventsCollection.AsQueryable()
                        where q.DriverId == id
                        select q;

            return query.ToListAsync();
        }

        public Task<Driver> GetDriverFromDb(string id)
        {
            var filter = Builders<Driver>.Filter.Eq(t => t.DriverId, id);
            return this.DriverCollection.Find(filter).FirstAsync();
        }

        public void Dispose()
        {
            this.driversToDelete.ForEach(id =>
            {
                this.DriverCollection.DeleteMany(t => t.DriverId == id);
                this.EventsCollection.DeleteMany(t => t.DriverId == id);
            });
        }
    }
}
