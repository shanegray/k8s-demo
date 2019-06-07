using DriverService.Events;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;

namespace DriverService.Models
{
    public class Driver
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public string DriverId { get; set; }
        public string Name { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        [BsonRepresentation(BsonType.String)]
        public DriverStatus Status { get; set; }
        public List<StatsUpdateEvent> LatestStats { get; set; } = new List<StatsUpdateEvent>();
        public StatsUpdateEvent OverallStats { get; set; } = new StatsUpdateEvent();
    }
}
