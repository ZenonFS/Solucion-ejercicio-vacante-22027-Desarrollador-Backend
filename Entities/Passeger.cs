using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Newtonsoft.Json;

namespace TravelAgencyApp.Entities
{
    public class Passeger
    {
        [BsonElement("_id")]
        [JsonProperty("_id")]
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string Names { get; set; }
        public string Surnames { get; set; }
        public DateTime? BirthDate { get; set; }
        public double? NumberDocument { get; set; }
        public string? Email { get; set; }
        public string ContactNumber { get; set; }
        public EmergencyContact EmergencyContact { get; set; }
    }
}
