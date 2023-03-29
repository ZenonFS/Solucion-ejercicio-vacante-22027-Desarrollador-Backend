using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Newtonsoft.Json;

namespace TravelAgencyApp.Entities
{
    public class EmergencyContact
    {
        [BsonElement("_id")]
        [JsonProperty("_id")]
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string Names { get; set; }
        public string Surnames { get; set; }
        public string ContactNumber { get; set; }
    }
}
