using DriverService.Config;
using DriverService.Events;
using DriverService.Models;
using DriverService.MongoConverters;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace DriverService.Service
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

        public async Task Hire(HiredEvent eventData)
        {
            await this.Insert(eventData.DriverId, DriverEventType.Hired, eventData);
        }

        public async Task LoadVan(string driverId, TimedEvent evtData)
        {
            evtData.Time = DateTime.UtcNow;
            await this.Insert(driverId, DriverEventType.LoadingVan, evtData);
        }

        private async Task Insert(string driverId, DriverEventType eventType, object data)
        {
            var evt = new EventStore
            {
                DriverId = driverId,
                Event = eventType,
                Data = new ObjectToBsonDocument(data).Convert()
            };

            await this.collection.InsertOneAsync(evt);
        }
    }
}
