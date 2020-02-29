using System;
using System.Linq;
using CityInfo.API.Services;
using CityInfo.API.Utility;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesInfoController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ICityInfoRepository _repository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        public CitiesInfoController(ICityInfoRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetCities()
        {
            var cities = _repository.GetCities();
            return Ok(CitiesDataStore.Default.Cities);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult GetCity(int id)
        {
            var cityToReturn = CitiesDataStore.Default.Cities.FirstOrDefault(c => c.Id == id);

            if(cityToReturn == null)
            {
                return NotFound();
            }
            return Ok(cityToReturn);
        }
    }
}