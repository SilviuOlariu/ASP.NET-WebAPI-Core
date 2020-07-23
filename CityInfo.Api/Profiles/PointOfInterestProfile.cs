using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.Api.Profiles
{
    public class PointOfInterestProfile: Profile
    {
        public PointOfInterestProfile()
        {
            CreateMap<Entities.PointOfInterest, Models.PointOfInterestDto>();
            CreateMap<Models.PointOfInterestForCreatingDto, Entities.PointOfInterest>();
            CreateMap<Models.PointOfInterestForUpdatingDto, Entities.PointOfInterest>()
                .ReverseMap();
            

        }

    }
}
