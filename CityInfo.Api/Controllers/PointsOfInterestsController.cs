using CityInfo.Api.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.Api.Controllers
{
    [ApiController]
    [Route("api/cities/{cityId}/pointOfInterest")]
    public class PointsOfInterestsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetPointOfInterest(int cityId)
        {

            var city = CityDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null || city.PointsOfInterests.Count == 0)
            {
                return NotFound();
            }

            return Ok(city.PointsOfInterests);
        }
        [HttpGet("{id}", Name = "GetPointsOfInterests")]
        public IActionResult GetPointOfInterest(int cityId, int id)
        {
            var city = CityDataStore.Current.Cities.FirstOrDefault(a => a.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }
            var pointofInterest = city.PointsOfInterests.FirstOrDefault(a => a.Id == id);
            if (pointofInterest == null)
            {
                return NotFound();
            }
            return Ok(pointofInterest);
        }
        [HttpPost]
        public IActionResult InsertPointOfInterest(int cityId, [FromBody] PointOfInterestForCreatingDto pointOfInterestForCreatingDTO)
        {
            if (pointOfInterestForCreatingDTO.Name == pointOfInterestForCreatingDTO.Description)
            {
                ModelState.AddModelError("Description",
                    "description should be different from name");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var city = CityDataStore.Current.Cities.FirstOrDefault(a => a.Id == cityId);
            if (city == null)
            {
                return BadRequest();
            }
            var maxId = CityDataStore.Current.Cities.SelectMany(p => p.PointsOfInterests).Max(a => a.Id);

            var pointOfInterestDto = new PointOfInterestDto
            {
                Id = ++maxId,
                Name = pointOfInterestForCreatingDTO.Name,
                Description = pointOfInterestForCreatingDTO.Description
            };

            city.PointsOfInterests.Add(pointOfInterestDto);

            return CreatedAtRoute("GetPointsOfInterests", new { cityId, id = pointOfInterestDto.Id }, pointOfInterestDto);

        }

        [HttpPut("{Id}")]
        public IActionResult UpdatePointOfInterest(int cityId, int id, [FromBody] PointOfInterestForCreatingDto pointOfInterestForCreatingDTO)
        {
            var city = CityDataStore.Current.Cities.FirstOrDefault(a => a.Id == cityId);
            if(city == null)
            {
                return BadRequest();
            }
            var pointToUpdate = city.PointsOfInterests.FirstOrDefault(a => a.Id == id);
            if(pointToUpdate == null)
            {
                return BadRequest();
            }
           
            pointToUpdate.Description = pointOfInterestForCreatingDTO.Description;
            pointToUpdate.Name = pointOfInterestForCreatingDTO.Name;

            return NoContent();

        }

        [HttpPatch("{id}")]
        public IActionResult PartiallyUpdatePointOfInterest(int cityId, 
            int id, [FromBody] JsonPatchDocument<PointOfInterestForUpdatingDto> patchdoc)
        {
            var city = CityDataStore.Current.Cities.FirstOrDefault(a => a.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }
            var pointOfInterestFromStore = city.PointsOfInterests.FirstOrDefault(a => a.Id == id);
            if (pointOfInterestFromStore == null)
            {
                return NotFound();
            }

            var pointOfInterestToPatch = new PointOfInterestForUpdatingDto()
            {
                Name = pointOfInterestFromStore.Name,
                Description = pointOfInterestFromStore.Description
            };

            patchdoc.ApplyTo(pointOfInterestToPatch, ModelState);
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
           if(!TryValidateModel(pointOfInterestToPatch))
            {
                return BadRequest(ModelState);
            }

            pointOfInterestFromStore.Name = pointOfInterestToPatch.Name;
            pointOfInterestFromStore.Description = pointOfInterestToPatch.Description;
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePointOfInterest(int cityId, int id)
        {
            var city = CityDataStore.Current.Cities.FirstOrDefault(a => a.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }
            var pointOfInterestFromStore = city.PointsOfInterests.FirstOrDefault(a => a.Id == id);
            if (pointOfInterestFromStore == null)
            {
                return NotFound();
            }

            city.PointsOfInterests.Remove(pointOfInterestFromStore);

            return NoContent();
        }
    }
}
