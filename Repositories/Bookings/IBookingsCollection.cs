using TravelAgencyApp.Entities;
using TravelAgencyApp.Models;

namespace TravelAgencyApp.Repositories.Bookings
{
    public interface IBookingsCollection
    {
        Task<List<Booking>> List();
        Task<Booking?> DetalisById(string id);
        Task<Booking> Book(Booking booking);
        Task<List<Booking>> ListOfBedroomsId(DateTime startDate, DateTime endDate);
    }
}
