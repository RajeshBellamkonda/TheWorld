using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheWorld.Models
{
    public class WorldRepository : IWorldRepository
    {
        private WorldContext _context;

        public WorldRepository(WorldContext context)
        {
            _context = context;
        }

        public void AddStop(string tripName, Stop stopEntity)
        {
            var trip = GetTripByName(tripName);
            if (trip != null)
            {
                trip.Stops.Add(stopEntity);
                _context.Stops.Add(stopEntity);
            }
        }

        public void AddStop(string tripName, Stop stopEntity, string name)
        {
            var trip = GetUserTripByName(tripName, name);
            if (trip != null)
            {
                trip.Stops.Add(stopEntity);
                _context.Stops.Add(stopEntity);
            }
        }

        public void AddTrip(Trip trip)
        {
            _context.Add(trip);
        }

        public IEnumerable<Trip> GetAllTrips()
        {
            return _context.Trips.ToList();
        }

        public Trip GetTripByName(string tripName)
        {
            return _context.Trips
                .Include(t => t.Stops)
                .Where(t => t.Name.ToLower() == tripName.ToLower())
                .FirstOrDefault();
        }

        public IEnumerable<Trip> GetTripsByUserName(string name)
        {
            return _context.Trips
                .Include(t => t.Stops)
                .Where(t => t.UserName == name)
                .ToList();
        }

        public Trip GetUserTripByName(string tripName, string name)
        {
            return _context.Trips
                .Include(t => t.Stops)
                .Where(t => t.Name.ToLower() == tripName.ToLower() && t.UserName.ToLower() == name.ToLower())
                .FirstOrDefault();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}
