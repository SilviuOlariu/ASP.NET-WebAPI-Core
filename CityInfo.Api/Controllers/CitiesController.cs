using AutoMapper;
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
        private readonly IMapper _mapper;


        public CitiesController(ICityInfoRepository cityInfoRepository, IMapper mapper)
        {
            _cityInfoRepository = cityInfoRepository;
            _mapper = mapper;
        }
       [HttpGet]
        public IActionResult GetCity()
        {
            var cityEntities = _cityInfoRepository.GetCities();

            //var results = new List<CityWithoutPointsOfInterestDto>();

            //foreach(var item in cityEntities)
            //{
            //    results.Add(new CityWithoutPointsOfInterestDto
            //    {
            //        Id = item.Id,
            //        Description = item.Description,
            //        Name = item.Name
            //    });
            //}

            return Ok(_mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>>(cityEntities));
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
                var cityresult = _mapper.Map<CityDto>(city);
                
                return Ok(cityresult);
            }
           
            return Ok(_mapper.Map<CityWithoutPointsOfInterestDto>(city));
           
        }
    }
}
