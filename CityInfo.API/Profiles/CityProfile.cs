using AutoMapper;
using CityInfo.API.Data.Entities;
using CityInfo.API.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Profiles
{
    public class CityProfile : Profile
    {
        public CityProfile()
        {
            CreateMap<Data.Entities.City, CityBase>();
            CreateMap<Data.Entities.City, Dto.City>();
        }
    }
}
