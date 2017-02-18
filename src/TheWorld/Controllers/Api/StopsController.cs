using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheWorld.Models;
using TheWorld.Services;
using TheWorld.ViewModels;

namespace TheWorld.Controllers.Api
{
    [Authorize]
    [Route("api/trips/{tripName}/stops")]
    public class StopsController : Controller
    {
        private GeoCoordsService _coordsService;
        private ILogger<StopsController> _logger;
        private IWorldRepository _repository;

        public StopsController(IWorldRepository repository, ILogger<StopsController> logger, GeoCoordsService coordsService)
        {
            _repository = repository;
            _logger = logger;
            _coordsService = coordsService;
        }
        [HttpGet("")]
        public IActionResult Get(string tripName)
        {
            try
            {
                var trip = _repository.GetUserTripByName(tripName, User.Identity.Name);
                return Ok(Mapper.Map<IEnumerable<StopViewModel>>(trip.Stops.OrderBy(s => s.Order).ToList()));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured when Getting Stops by tripName {tripName} {ex}");
                return BadRequest($"Error occured when Getting Stops by tripName {tripName}");
            }
        }

        [HttpPost("")]
        public async Task<IActionResult> Post(string tripName, [FromBody]StopViewModel vm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var stopEntity = Mapper.Map<Stop>(vm);

                    var result = await _coordsService.GetCoordsAsync(stopEntity.Name);
                    if (result.Success)
                    {
                        stopEntity.Latitude = result.Latitude;
                        stopEntity.Longitude = result.Longitude;
                        
                        _repository.AddStop(tripName, stopEntity, User.Identity.Name);
                        if (await _repository.SaveChangesAsync())
                            return Created($"/api/trips/{tripName}/stops/{stopEntity.Name}",
                                Mapper.Map<StopViewModel>(stopEntity));
                    }else
                    {
                        _logger.LogError(result.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to save new Stop {ex}");
            }
            return BadRequest("Failed to save new stop");
        }
    }
}
