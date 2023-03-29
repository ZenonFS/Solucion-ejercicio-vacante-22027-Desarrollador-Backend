using MongoDB.Bson;
using MongoDB.Driver;
using TravelAgencyApp.Entities;
using TravelAgencyApp.Models;

namespace TravelAgencyApp.Repositories.Bookings
{
    public class BookingsCollection : IBookingsCollection
    {
        internal MongoDBRepository _repository = new MongoDBRepository();
        private IMongoCollection<Booking> Collection;

        public BookingsCollection()
        {
            Collection = _repository.database.GetCollection<Booking>("Bookings");
        }

        public async Task<Booking> Book(Booking booking)
        {
            booking.Id = ObjectId.GenerateNewId().ToString();
            foreach (Passeger passeger in booking.Passagers)
            {
                passeger.Id = ObjectId.GenerateNewId().ToString();
                passeger.EmergencyContact.Id = ObjectId.GenerateNewId().ToString();
            }

            await Collection.InsertOneAsync(booking);
            return booking;
        }

        public async Task<Booking?> DetalisById(string id)
        {
            return await Collection.FindAsync(new BsonDocument { { "_id", new ObjectId(id) } }).Result.FirstAsync();
        }

        

        public async Task<List<Booking>> List()
        {
            var sort = Builders<Booking>.Sort.Descending("_id");
            var sortedDocuments = Collection.Find(x => true).Sort(sort).ToList();

            return sortedDocuments.ToList();
            
        }

        public async Task<List<Booking>> ListOfBedroomsId(DateTime startDate, DateTime endDate)
        {
            var filter =  Builders<Booking>.Filter.And(
                Builders<Booking>.Filter.Gte(x => x.StartDate, startDate),
                Builders<Booking>.Filter.Lte(x => x.EndDate, endDate)
            );  
            return await Collection.Find(filter).ToListAsync();
        }
    }
}
