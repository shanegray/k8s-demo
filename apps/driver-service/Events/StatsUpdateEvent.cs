using MongoDB.Bson.Serialization.Attributes;

namespace DriverService.Events
{
    public class StatsUpdateEvent
    {
        [BsonElement("pods")]
        public decimal PODs { get; set; }
        [BsonElement("dncs")]
        public decimal DNCs { get; set; }
    }
}
