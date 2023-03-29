using TravelAgencyApp.Models;

namespace TravelAgencyApp.Repositories.Bedrooms
{
    public interface IBedroomsCollection
    {
        Task<Bedroom> Update(Bedroom bedroom);
        Task<Bedroom> Inactivate(string id);
        
        Task<Bedroom> Activate(string id);
        Task<Bedroom> ChangeStatus(string id, bool status);
        Task<Bedroom> DetalisById(string id);
    }
}
