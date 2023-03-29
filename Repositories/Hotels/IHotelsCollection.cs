using TravelAgencyApp.Models;

namespace TravelAgencyApp.Repositories.Collections
{
    public interface IHotelsCollection
    {
        Task<Hotel> Insert(Hotel hotel);
        Task<Hotel> Update(Hotel hotel);
        Task<Hotel> Inactivate(string id);
        Task<Hotel> Activate(string id);
        Task<Hotel> ChangeStatus(string id, bool status);

        Task<List<Hotel>> List(DateTime? startDate, DateTime? endDate, int? numberPassagers, string? city);
        Task<Hotel> DetalisById(string id);
    }
}
