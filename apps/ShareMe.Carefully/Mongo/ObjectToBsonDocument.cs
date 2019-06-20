using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using Newtonsoft.Json;

namespace ShareMe.Carefully
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
            var data = JsonConvert.SerializeObject(this.theObject);
            return BsonSerializer.Deserialize<BsonDocument>(data);
        }
    }
}
