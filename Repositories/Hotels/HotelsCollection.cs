using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Concurrent;
using System.Globalization;
using TravelAgencyApp.Entities;
using TravelAgencyApp.Models;
using TravelAgencyApp.Repositories.Bookings;

namespace TravelAgencyApp.Repositories.Collections
{
    public class HotelsCollection : IHotelsCollection
    {
        internal MongoDBRepository _repository = new MongoDBRepository();
        private IMongoCollection<Hotel> Collection;

        private IBookingsCollection bookingCollection = new BookingsCollection();

        public HotelsCollection ()
        {
            Collection = _repository.database.GetCollection<Hotel>("Hotels");
        }
        public async Task<Hotel> Insert(Hotel hotel)
        {
            hotel.IsActive = true;
            foreach (Bedroom bedroom in hotel.Bedrooms)
            {
                bedroom.Id = ObjectId.GenerateNewId().ToString();
                bedroom.IsActive = true;
            }
            
            await Collection.InsertOneAsync(hotel);
            return hotel;
        }
        public async Task<Hotel> DetalisById(string id)
        {
            return await Collection.FindAsync(new BsonDocument { { "_id", new ObjectId(id) } }).Result.FirstAsync();
        }
        public async Task<List<Hotel>> List(DateTime? startDate, DateTime? endDate, int? numberPassagers, string? city)
        {
            var sort = Builders<Hotel>.Sort.Descending("_id");

            if (city == null && numberPassagers == null && endDate == null && startDate == null)
            {
                var documents = Collection.Find(x => true).Sort(sort).ToList();

                return documents.ToList();
            }
            var cityFilter = Builders<Hotel>.Filter.Eq(
                x => x.City, city);


            List<FilterDefinition<Hotel>> listDatesFilter = new List<FilterDefinition<Hotel>>();
            if (startDate != null && endDate != null)
            {
                List<Booking> bookings = await bookingCollection.ListOfBedroomsId((DateTime)startDate, (DateTime)endDate);
                foreach (Booking book in bookings)
                {
                    listDatesFilter.Add(
                        Builders<Hotel>.Filter.Regex
                            (x => x.Id, new BsonRegularExpression("^(?!.*" + book.Id + ").*$"))
                        );

                }
            }
            FilterDefinition<Hotel>? datesFilter = null;
            if (listDatesFilter.Count > 0)
                datesFilter = Builders<Hotel>.Filter.And(listDatesFilter);

            var passagersQuantityFilter = Builders<Hotel>.Filter.ElemMatch(
                x => x.Bedrooms, y => y.MaxPeople <= numberPassagers);

            //var filters = datesFilter | cityFilter | passagersQuantityFilter;

            var filters = Builders<Hotel>.Filter.Or(cityFilter, passagersQuantityFilter, datesFilter);
        
            try
            {
                var sortedDocuments = Collection.Find(filters).Sort(sort);
                    if (sortedDocuments == null) return new List<Hotel>();
                return sortedDocuments.ToList();
            }
            catch (IOException e)
            {

                if (e.Source != null)
                    Console.WriteLine("IOException source: {0}", e.Source);
                throw;
            }

        }

        public async Task<Hotel> Activate(string id)
        {
            return await ChangeStatus(id, true);
        }


        public async Task<Hotel> Inactivate(string id)
        {
            return await ChangeStatus(id, false);
        }

        public async Task<Hotel> ChangeStatus(string id, bool status)
        {
            var filter = Builders<Hotel>.Filter.Eq(s => s.Id, id);
            Hotel hotel = await DetalisById(id);
            hotel.IsActive = status;
            await Collection.ReplaceOneAsync(filter, hotel);
            return hotel;
        }



        public async Task<Hotel> Update(Hotel hotel)
        {
            var filter = Builders<Hotel>.Filter.Eq(s => s.Id, hotel.Id);
            await Collection.ReplaceOneAsync(filter, hotel);
            return hotel;
        }
    }
}
