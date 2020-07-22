using CityInfo.Api.Models;
using CityInfo.Api.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.Api.Controllers
{
    [ApiController]
    [Route("api/cities")]
    public class CitiesController: ControllerBase
    {
        private readonly ICityInfoRepository _cityInfoRepository;

        public CitiesController(ICityInfoRepository cityInfoRepository)
        {
            _cityInfoRepository = cityInfoRepository;
        }
       [HttpGet]
        public IActionResult GetCity()
        {
            var cityEntities = _cityInfoRepository.GetCities();

            var results = new List<CityWithoutPointsOfInterestDto>();

            foreach(var item in cityEntities)
            {
                results.Add(new CityWithoutPointsOfInterestDto
                {
                    Id = item.Id,
                    Description = item.Description,
                    Name = item.Name
                });
            }
            return Ok(results);
        }
      
        [HttpGet("{id}")]
        public IActionResult GetCity(int id, bool includePointOfInterest = false)
        {
            
           var city = _cityInfoRepository.GetCity(id, includePointOfInterest);
            if(city == null)
            {
                return NotFound();
            }
            if(includePointOfInterest)
            {
                var cityresult = new CityDto()
                {
                    Id = city.Id,
                    Name = city.Name,
                    Description = city.Description
                };
                foreach(var item in city.PointsOfInterest)
                {
                    cityresult.PointsOfInterests.Add(new PointOfInterestDto
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Description = item.Description
                    });
                }
                return Ok(cityresult);
            }
            var citWithoutPoints = new CityWithoutPointsOfInterestDto()
            {
                Id = city.Id,
                Name = city.Name,
                Description = city.Description

            };
            return Ok(citWithoutPoints);
           
        }
    }
}
