using CityInfo.Api.Models;
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
       [HttpGet]
        public IActionResult GetCity()
        {
            return Ok(CityDataStore.Current.Cities.ToList());
              
        }
      
        [HttpGet("{id}")]
        public IActionResult GetCity(int id)
        {
            var citylist = CityDataStore.Current.Cities;
            foreach (var city in citylist)
            {
                if (id == city.Id)

                    return Ok(city);
            }
            return NotFound();
        }
    }
}
