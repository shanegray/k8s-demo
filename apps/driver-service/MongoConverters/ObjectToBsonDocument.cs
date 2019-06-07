using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DriverService.MongoConverters
{
    public class ObjectToBsonDocument
    {
        private readonly object theObject;

        public ObjectToBsonDocument(object theObject)
        {
            this.theObject = theObject;
        }

        public BsonDocument Convert()
        {
            var serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            var data = JsonConvert.SerializeObject(this.theObject, serializerSettings);
            return BsonSerializer.Deserialize<BsonDocument>(data);
        }
    }
}
