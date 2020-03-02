using CityInfo.API.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Services
{
    public interface ICityInfoRepository
    {
        IEnumerable<City> GetCities();

        City GetCity(int cityId, bool hasToIncludePointOfInterest);

        IEnumerable<PointOfInterest> GetPointOfInterestsForCity(int cityId);

        PointOfInterest GetPointOfInterestForCity(int cityId, int pointOfInterestId);

        bool IsCityExists(int id);

        void AddPointOfInterestForCity(int cityId, PointOfInterest entity);

        void UpdatePointOfInterestForCity(int cityId, PointOfInterest entity);

        void DeletePointOfInterest(PointOfInterest pointOfInterest);
    }
}
