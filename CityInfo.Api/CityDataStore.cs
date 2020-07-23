using CityInfo.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.Api
{
    public class CityDataStore
    {
        public static CityDataStore Current { get; } = new CityDataStore();
        public List<CityDto> Cities { get; set; }
        
        public CityDataStore()
        {
            Cities = new List<CityDto>()
            {
                new CityDto {Id = 1, Name = "Cluj-Napoca", Description = "The most expenise city from Romania",
                    PointsOfInterest =
                    new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto {Id=1, Description = "The Square", Name ="main square"},
                         new PointOfInterestDto {Id=2, Description = "The second", Name ="sample2"}

                    }},

                new CityDto {Id = 2, Name ="Iasi", Description ="The capital of Moldova",
                  PointsOfInterest =
                    new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto {Id=3, Description = "Sample3", Name ="sample name"},
                         new PointOfInterestDto {Id=4, Description = "The third", Name ="sample3"}
                } },

                new CityDto {Id =3, Name ="Bucuresti", Description ="The capital of Romania"},

                new CityDto {Id =4, Name ="Suceava", Description = "The biggest city of Bucovina"}
            };
            
    
        }
    }
}
