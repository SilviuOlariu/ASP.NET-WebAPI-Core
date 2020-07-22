using CityInfo.Api.Models;
using CityInfo.Api.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<PointsOfInterestsController> _logger;
        private readonly IMailService _mailService;
        private readonly ICityInfoRepository _cityInfoRepository;

        public PointsOfInterestsController(ILogger<PointsOfInterestsController> logger, IMailService mailService, ICityInfoRepository cityInfoRepository)
        {
            _logger = logger;
            _mailService = mailService;
            _cityInfoRepository = cityInfoRepository;
        }
        [HttpGet]
        public IActionResult GetPointOfInterest(int cityId)
        {
            bool checkcity = _cityInfoRepository.CityExists(cityId);
            if(!checkcity)
            {
                _logger.LogInformation($"City with id {cityId} wasn't found when " + $"accessing points of interest");
                return NotFound();
            }
            var pointsEntities = _cityInfoRepository.GetPointsOfInterestForCity(cityId);

            var result = new List<PointOfInterestDto>();
               
                 foreach( var item in pointsEntities)
                 {
                result.Add(new PointOfInterestDto()
                 {
                    Id = item.Id,
                    Description = item.Description,
                    Name = item.Name
                 });
               
                 }
            return Ok(result);




        }
        [HttpGet("{id}", Name = "GetPointsOfInterests")]
        public IActionResult GetPointOfInterest(int cityId, int id)
        {
            if (!_cityInfoRepository.CityExists(cityId))
            {
                return NotFound();
            }
            var pointEntity = _cityInfoRepository.GetPointOfInterestForCity(cityId, id);
            if(pointEntity == null)
            {
                return NotFound();
            }
            var pointOfInterest = new PointOfInterestDto
            {
                Id = pointEntity.Id,
                Name = pointEntity.Name,
                Description = pointEntity.Description
            };
            return Ok(pointOfInterest);
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

            _mailService.SendMail("point deleted", "your point of interest has been deleted");

            return NoContent(); 
        }
    }
}
