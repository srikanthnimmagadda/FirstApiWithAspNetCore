using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CityInfo.API.Dto;
using CityInfo.API.Services;
using CityInfo.API.Utility;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CityInfo.API.Controller
{
    [Route("api/citiesinfo/{cityId}/[controller]")]
    [ApiController]
    public class PointsOfInterestController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ILogger<PointsOfInterestController> _logger;

        /// <summary>
        /// 
        /// </summary>
        private readonly IMailService _mailService;

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
        /// <param name="logger"></param>
        public PointsOfInterestController(ILogger<PointsOfInterestController> logger, IMailService mailService,
            ICityInfoRepository repositoy, IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mailService = mailService ?? throw new ArgumentNullException(nameof(mailService));
            _repository = repositoy ?? throw new ArgumentNullException(nameof(repositoy));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
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
                if (!_repository.IsCityExists(cityId))
                {
                    _logger.LogInformation($"City with id {cityId} was not found when accessing point of inetrests.");
                    return NotFound();
                }

                var pointOfInterestsForCity = _repository.GetPointOfInterestsForCity(cityId);
                var results = _mapper.Map<IEnumerable<PointOfInterest>>(pointOfInterestsForCity);
                return Ok(results);
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
            if (!_repository.IsCityExists(cityId))
            {
                return NotFound();
            }

            var pointOfInterest = _repository.GetPointOfInterestForCity(cityId, id);
            if (pointOfInterest == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<PointOfInterest>(pointOfInterest));
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

            if (!_repository.IsCityExists(cityId))
            {
                return BadRequest();
            }

            var entity = _mapper.Map<Data.Entities.PointOfInterest>(model);
            _repository.AddPointOfInterestForCity(cityId, entity);
            var createdPointOfInterest = _mapper.Map<PointOfInterest>(entity);

            return CreatedAtRoute("GetPointOfInterest",
                new { cityId, id = createdPointOfInterest.Id },
                createdPointOfInterest);
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
                ModelState.AddModelError("Description", "The provided Name and Description must be different");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_repository.IsCityExists(cityId))
            {
                return NotFound();
            }

            var entity = _repository.GetPointOfInterestForCity(cityId, id);
            if (entity == null)
            {
                return NotFound();
            }

            _mapper.Map(model, entity);
            _repository.UpdatePointOfInterestForCity(cityId, entity);
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
            if (!_repository.IsCityExists(cityId))
            {
                return NotFound();
            }

            var pointOfInterestEntity = _repository.GetPointOfInterestForCity(cityId, id);
            if (pointOfInterestEntity == null)
            {
                return NotFound();
            }


            var pointOfInterestToPatch = _mapper.Map<PointOfInterestUpdate>(pointOfInterestEntity);
            model.ApplyTo(pointOfInterestToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (pointOfInterestToPatch.Description == pointOfInterestToPatch.Name)
            {
                ModelState.AddModelError(
                    "Description",
                    "The provided description should be different from the name.");
            }

            if (!TryValidateModel(pointOfInterestToPatch))
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(pointOfInterestToPatch, pointOfInterestEntity);
            _repository.UpdatePointOfInterestForCity(cityId, pointOfInterestEntity);
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
            if (!_repository.IsCityExists(cityId))
            {
                return NotFound();
            }

            var pointOfInterestEntity = _repository.GetPointOfInterestForCity(cityId, id);
            if (pointOfInterestEntity == null)
            {
                return NotFound();
            }

            _repository.DeletePointOfInterest(pointOfInterestEntity);
            _mailService.Send("Point of interest deleted.",
                    $"Point of interest {pointOfInterestEntity.Name} with id {pointOfInterestEntity.Id} was deleted.");

            return NoContent();
        }
    }
}