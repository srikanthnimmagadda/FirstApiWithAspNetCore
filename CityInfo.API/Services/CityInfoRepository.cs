using CityInfo.API.Data;
using CityInfo.API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CityInfo.API.Services
{
    public class CityInfoRepository : ICityInfoRepository
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly CityInfoDbContext _dbContext;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        public CityInfoRepository(CityInfoDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="entity"></param>
        public void AddPointOfInterestForCity(int cityId, PointOfInterest entity)
        {
            var city = GetCity(cityId, false);
            city.PointOfInterest.Add(entity);
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<City> GetCities()
        {
            return _dbContext.Cities.OrderBy(c => c.Name).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public City GetCity(int cityId, bool hasToIncludePointOfInterest)
        {
            if (hasToIncludePointOfInterest)
            {
                return _dbContext.Cities.Include(c => c.PointOfInterest).Where(c => c.Id == cityId).FirstOrDefault();
            }

            return _dbContext.Cities.Where(c => c.Id == cityId).FirstOrDefault();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="pointOfInterestId"></param>
        /// <returns></returns>
        public PointOfInterest GetPointOfInterestForCity(int cityId, int pointOfInterestId)
        {
            return _dbContext.PointOfInterest.Where(p => p.CityId == cityId && p.Id == pointOfInterestId).FirstOrDefault();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public IEnumerable<PointOfInterest> GetPointOfInterestsForCity(int cityId)
        {
            return _dbContext.PointOfInterest.Where(p => p.CityId == cityId).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsCityExists(int id)
        {
            return _dbContext.Cities.Any(c => c.Id == id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="pointOfInterest"></param>
        public void UpdatePointOfInterestForCity(int cityId, PointOfInterest pointOfInterest)
        {
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pointOfInterest"></param>
        public void DeletePointOfInterest(PointOfInterest pointOfInterest)
        {
            _dbContext.PointOfInterest.Remove(pointOfInterest);
            _dbContext.SaveChanges();
        }
    }
}
