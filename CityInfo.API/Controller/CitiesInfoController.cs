using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CityInfo.API.Dto;
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
        private readonly IMapper _mapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        public CitiesInfoController(ICityInfoRepository repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetCities()
        {
            IEnumerable<Data.Entities.City> cities = _repository.GetCities();
            IEnumerable<CityBase> results = _mapper.Map<IEnumerable<CityBase>>(cities);
            return Ok(results);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="hasToIncludePointOfInterest"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult GetCity(int id, bool hasToIncludePointOfInterest = false)
        {
            var city = _repository.GetCity(id, hasToIncludePointOfInterest);

            if (city == null)
            {
                return NotFound();
            }

            if(hasToIncludePointOfInterest)
            {
                return Ok(_mapper.Map<City>(city));
            }

            return Ok(_mapper.Map<CityBase>(city));
        }
    }
}