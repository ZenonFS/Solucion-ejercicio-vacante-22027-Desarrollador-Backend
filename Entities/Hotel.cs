using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace TravelAgencyApp.Models
{
    public class Hotel
    {
        [BsonElement("_id")]
        [JsonProperty("_id")]
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public List<Bedroom> Bedrooms { get; set; }
        public bool IsActive { get; set; }
    }
}
