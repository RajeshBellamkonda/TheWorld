using System.Collections.Generic;
using System.Threading.Tasks;

namespace TheWorld.Models
{
    public interface IWorldRepository
    {
        IEnumerable<Trip> GetAllTrips();
        IEnumerable<Trip> GetTripsByUserName(string name);
        void AddTrip(Trip trip);
        void AddStop(string tripName, Stop stopEntity);
        Task<bool> SaveChangesAsync();
        Trip GetTripByName(string tripName);
        Trip GetUserTripByName(string tripName, string name);
        void AddStop(string tripName, Stop stopEntity, string name);
    }
}