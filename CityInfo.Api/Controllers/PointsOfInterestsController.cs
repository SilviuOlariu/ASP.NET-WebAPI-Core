using CityInfo.Api.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.Api.Controllers
{
    [ApiController]
    [Route("api/cities/{cityId}/pointOfInterest")]
    public class PointsOfInterestsController:ControllerBase
    {
        [HttpGet]
        public IActionResult GetPointOfInterest(int cityId)
        {
         
           var city = CityDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if(city == null || city.PointsOfInterests.Count == 0)
             { 
                 return NotFound(); 
             }
            
               return Ok(city. PointsOfInterests);
        }
        [HttpGet("{id}")]
        public IActionResult GetPointOfInterest(int cityId, int id)
        {
            var city = CityDataStore.Current.Cities.FirstOrDefault(a => a.Id == cityId);
            if(city == null)
            {
                return NotFound();
            }
            var pointofInterest = city.PointsOfInterests.FirstOrDefault(a => a.Id == id);
            if(pointofInterest == null)
            {
                return NotFound();
            }
            return Ok(pointofInterest);
        }
       
    }
}
