using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DriverService.Models
{
    public class EventStore
    {
        public ObjectId Id { get; set; }
        
        // Unique
        public string DriverId { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        [BsonRepresentation(BsonType.String)]
        public DriverEventType Event { get; set; }

        public BsonDocument Data { get; set; }
    }
}
