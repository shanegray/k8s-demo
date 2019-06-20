using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace PackageService.Models
{
    public class Package
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public string Barcode { get; set; }
        public PackageState State { get; set; }
        public Person Customer { get; set; }
        // only for demo purposes
        public List<Address> Addresses { get; set; } = new List<Address>();

        [BsonIgnoreIfNull]
        public Signature Signature { get; set; }

        [BsonIgnoreIfNull]
        public BsonDocument Location { get; set; }

        [BsonIgnoreIfNull]
        public BsonDocument Notes { get; set; }
    }
}
