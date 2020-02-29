using System;
using System.Linq;
using CityInfo.API.Dto;
using CityInfo.API.Services;
using CityInfo.API.Utility;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CityInfo.API.Controller
{
    [Route("api/cities/{cityId}/[controller]")]
    [ApiController]
    public class PointsOfInterestController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ILogger<PointsOfInterestController> _logger;
        private readonly IMailService _mailService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public PointsOfInterestController(ILogger<PointsOfInterestController> logger, IMailService mailService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mailService = mailService ?? throw new ArgumentNullException(nameof(mailService));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetPointsOfInterest(int cityId)
        {
            try
            {
                //throw new Exception("Something wrong");
                var city = CitiesDataStore.Default.Cities.FirstOrDefault(c => c.Id == cityId);

                if (city == null)
                {
                    _logger.LogInformation($"City with id {cityId} was not found when accessing point of inetrests.");
                    return NotFound();
                }

                return Ok(city.PointsOfInterest);
            }
            catch (Exception exception)
            {
                _logger.LogCritical($"Exception while getting point of interest for City with id {cityId}.", exception);
                return StatusCode(500, "A problem happend while handling your request.");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetPointOfInterest")]
        public IActionResult GetPointOfInterest(int cityId, int id)
        {
            var city = CitiesDataStore.Default.Cities.FirstOrDefault(c => c.Id == cityId);

            if (city == null)
            {
                return NotFound();
            }

            var pointOfInterestToReturn = city.PointsOfInterest.FirstOrDefault(p => p.Id == id);

            if (pointOfInterestToReturn == null)
            {
                return NotFound();
            }

            return Ok(pointOfInterestToReturn);
        }

        /// <summary>
        /// Creating a resource
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Create(int cityId, PointOfInterestCreate model)
        {
            if (model.Name.Equals(model.Description, StringComparison.OrdinalIgnoreCase))
            {
                ModelState.AddModelError("Description", "The provided Nmae and Description must be different");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var city = CitiesDataStore.Default.Cities.FirstOrDefault(c => c.Id == cityId);

            if (city == null)
            {
                return BadRequest();
            }

            //Demo purpose only - it can be avoided after entity framework is added
            int maxId = CitiesDataStore.Default.Cities.SelectMany(c => c.PointsOfInterest).Max(p => p.Id);
            var finalPointOfInterest = new PointOfInterest
            {
                Id = ++maxId,
                Name = model.Name,
                Description = model.Description
            };

            city.PointsOfInterest.Add(finalPointOfInterest);
            return CreatedAtRoute("GetPointOfInterest", new { cityId, id = finalPointOfInterest.Id }, finalPointOfInterest);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public IActionResult Update(int cityId, int id, PointOfInterestUpdate model)
        {
            if (model.Name.Equals(model.Description, StringComparison.OrdinalIgnoreCase))
            {
                ModelState.AddModelError("Description", "The provided Nmae and Description must be different");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var city = CitiesDataStore.Default.Cities.FirstOrDefault(c => c.Id == cityId);

            if (city == null)
            {
                return BadRequest();
            }

            var pointOfInterest = city.PointsOfInterest.FirstOrDefault(p => p.Id == id);

            if (pointOfInterest == null)
            {
                return BadRequest();
            }

            pointOfInterest.Name = model.Name;
            pointOfInterest.Description = model.Description;

            return NoContent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        public IActionResult PartiallyUpdatePointOfInterest(int cityId, int id, JsonPatchDocument<PointOfInterestUpdate> model)
        {
            var city = CitiesDataStore.Default.Cities.FirstOrDefault(c => c.Id == cityId);

            if (city == null)
            {
                return NotFound();
            }

            var pointOfInterest = city.PointsOfInterest.FirstOrDefault(p => p.Id == id);

            if (pointOfInterest == null)
            {
                return NotFound();
            }

            var pointOfInterestToPatch = new PointOfInterestUpdate
            {
                Name = pointOfInterest.Name,
                Description = pointOfInterest.Description
            };

            model.ApplyTo(pointOfInterestToPatch);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (pointOfInterestToPatch.Name.Equals(pointOfInterestToPatch.Description, StringComparison.OrdinalIgnoreCase))
            {
                ModelState.AddModelError("Description", "The provided Nmae and Description must be different");
            }

            if (!TryValidateModel(pointOfInterestToPatch))
            {
                return BadRequest(ModelState);
            }

            pointOfInterest.Name = pointOfInterestToPatch.Name;
            pointOfInterest.Description = pointOfInterestToPatch.Description;

            return NoContent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult DeletePointOfInterest(int cityId, int id)
        {
            var city = CitiesDataStore.Default.Cities.FirstOrDefault(c => c.Id == cityId);

            if (city == null)
            {
                return NotFound();
            }

            var pointOfInterest = city.PointsOfInterest.FirstOrDefault(p => p.Id == id);

            if (pointOfInterest == null)
            {
                return NotFound();
            }

            city.PointsOfInterest.Remove(pointOfInterest);
            _mailService.Send("Point of Interest has been deleted", $"Point of Interest {pointOfInterest.Name} with id {pointOfInterest.Id} was deleted.");

            return NoContent();
        }
    }
}