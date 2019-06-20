using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PackageService.Models
{
    public class Address
    {

        [JsonConverter(typeof(StringEnumConverter))]
        [BsonRepresentation(BsonType.String)]
        public AddressType AddressType { get; set; }
        public string AddressText { get; set; }
        public string Country { get; set; }
    }

    public enum AddressType
    {
        Delivery,
        Pickup,
        Other
    }
}
