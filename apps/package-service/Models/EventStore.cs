using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PackageService.Events;

namespace PackageService.Models
{
    public class EventStore
    {
        public ObjectId Id { get; set; }

        // Unique
        public string Barcode { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        [BsonRepresentation(BsonType.String)]
        public PackageEventType Event { get; set; }

        public BsonDocument Data { get; set; }
    }
}
