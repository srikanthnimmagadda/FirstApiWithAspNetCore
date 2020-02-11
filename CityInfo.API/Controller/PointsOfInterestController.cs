using System;
using System.Linq;
using CityInfo.API.Dto;
using CityInfo.API.Utility;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controller
{
    [Route("api/cities/{cityId}/[controller]")]
    [ApiController]
    public class PointsOfInterestController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetPointsOfInterest(int cityId)
        {
            var city = CitiesDataStore.Default.Cities.FirstOrDefault(c => c.Id == cityId);

            if (city == null)
            {
                return NotFound();
            }

            return Ok(city.PointsOfInterest);
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
    }
}