using MongoDB.Bson;
using MongoDB.Driver;
using System.Reflection;
using TravelAgencyApp.Models;

namespace TravelAgencyApp.Repositories.Bedrooms
{
    public class BedroomsCollection : IBedroomsCollection
    {
        internal MongoDBRepository _repository = new MongoDBRepository();
        private IMongoCollection<Hotel> Collection;

        public BedroomsCollection()
        {
            Collection = _repository.database.GetCollection<Hotel>("Hotels");
        }
        
        public async Task<Bedroom> Activate(string id)
        {
            return await ChangeStatus(id, true);
        }

        public async Task<Bedroom> Inactivate(string id)
        {
            return await ChangeStatus(id, false);
        }

        public async Task<Bedroom> Update(Bedroom bedroom)
        {
            var filter = Builders<Hotel>.Filter.ElemMatch(
            x => x.Bedrooms,
            y => y.Id == bedroom.Id);
            Hotel hotel = await Collection.FindAsync(filter).Result.FirstAsync();
            Bedroom lastBedroom = hotel.Bedrooms.FirstOrDefault(_bedroom => _bedroom.Id == bedroom.Id);

            if (lastBedroom == null)
            {

                hotel.Bedrooms.Add(bedroom);
            }
            else
            {
                PropertyInfo[] lst = typeof(Bedroom).GetProperties();
                foreach (PropertyInfo pi in lst)
                {
                    string valueBedroom = pi.GetValue(bedroom).ToString();
                    if (valueBedroom != pi.GetValue(lastBedroom).ToString())
                    {
                        pi.SetValue(lastBedroom, Convert.ChangeType(valueBedroom, pi.PropertyType), null);
                    }
                }
                
            }
           
            
            await Collection.ReplaceOneAsync(filter, hotel);
            return hotel.Bedrooms.Find(bedroom2 => bedroom2.Id == bedroom.Id);
        }
        public async Task<Bedroom> ChangeStatus(string id, bool status)
        {
            var filter = Builders<Hotel>.Filter.ElemMatch(
            x => x.Bedrooms,
            y => y.Id == id);

            Bedroom bedroom = await DetalisById(id);
            bedroom.IsActive = status;
            bedroom = await Update(bedroom);
            return bedroom;
        }

        public async Task<Bedroom> DetalisById(string id)
        {
            var filter = Builders<Hotel>.Filter.ElemMatch(
            x => x.Bedrooms,
            y => y.Id == id
            
        );
            Hotel hotel = await Collection.FindAsync(filter).Result.FirstAsync();
            return hotel.Bedrooms.Find(bedroom => bedroom.Id == id);
        }

        
    }
}
