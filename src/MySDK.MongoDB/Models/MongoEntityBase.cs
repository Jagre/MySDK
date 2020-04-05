using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MySDK.MongoDB.Models
{
    public abstract class MongoEntityBase
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }
}