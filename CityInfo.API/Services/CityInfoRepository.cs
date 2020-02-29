using CityInfo.API.Data;
using CityInfo.API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
    }
}
