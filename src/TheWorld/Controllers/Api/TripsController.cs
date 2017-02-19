using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheWorld.Models;
using TheWorld.ViewModels;

namespace TheWorld.Controllers.Api
{
    [Route("api/trips")]
    [Authorize]
    public class TripsController : Controller
    {
        private ILogger<TripsController> _logger;
        private IWorldRepository _repository;

        public TripsController(IWorldRepository repository, ILogger<TripsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }
        [HttpGet("")]
        public IActionResult Get()
        {
            try
            {
                var trips = _repository.GetTripsByUserName(this.User.Identity.Name).OrderBy(t => t.Name);
                return Ok(Mapper.Map<IEnumerable<TripViewModel>>(trips));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get All Trips {ex}");
                return BadRequest("Error occured");
            }
        }
        [HttpPost("")]
        public async Task<IActionResult> Post([FromBody]TripViewModel trip)
        {
            if (ModelState.IsValid)
            {
                var tripEntity = Mapper.Map<Trip>(trip);
                tripEntity.UserName = User.Identity.Name;
                _repository.AddTrip(tripEntity);
                if (await _repository.SaveChangesAsync())
                    return Created($"api/trips/{trip.Name}", Mapper.Map<TripViewModel>(tripEntity));
                else
                    return BadRequest("Failed to save changes to the database");
            }
            return BadRequest(ModelState);
        }
    }
}
