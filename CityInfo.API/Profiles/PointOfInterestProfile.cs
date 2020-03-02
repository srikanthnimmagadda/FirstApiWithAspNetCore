using AutoMapper;
using CityInfo.API.Data.Entities;
using CityInfo.API.Dto;

namespace CityInfo.API.Profiles
{
    public class PointOfInterestProfile : Profile
    {
        /// <summary>
        /// 
        /// </summary>
        public PointOfInterestProfile()
        {
            CreateMap<Data.Entities.PointOfInterest, Dto.PointOfInterest>();
            CreateMap<PointOfInterestCreate, Data.Entities.PointOfInterest>();
            CreateMap<PointOfInterestUpdate, Data.Entities.PointOfInterest>().ReverseMap();
        }
    }
}
