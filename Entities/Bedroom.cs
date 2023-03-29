using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Newtonsoft.Json;

namespace TravelAgencyApp.Models
{
    public class Bedroom
    {
        [BsonElement("_id")]
        [JsonProperty("_id")]
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public int Number { get; set; }
        public double BasisCost { get; set; }
        public double Taxes { get; set; }
        public string Type { get; set; }
        public string Location { get; set; }
        public bool IsActive { get; set; }
        public bool IsBooked { get; set; }

        public int MaxPeople { get; set; }
        

    }
}
