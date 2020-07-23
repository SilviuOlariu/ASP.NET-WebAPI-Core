using AutoMapper;
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
        private readonly IMapper _mapper;

        public PointsOfInterestsController(ILogger<PointsOfInterestsController> logger, IMailService mailService, ICityInfoRepository cityInfoRepository, IMapper mapper)
        {
            _logger = logger;
            _mailService = mailService;
            _cityInfoRepository = cityInfoRepository;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult GetPointsOfInterest(int cityId)
        {
            bool checkcity = _cityInfoRepository.CityExists(cityId);
            if(!checkcity)
            {
                _logger.LogInformation($"City with id {cityId} wasn't found when " + $"accessing points of interest");
                return NotFound();
            }
            var pointOfInterest = _cityInfoRepository.GetPointsOfInterestForCity(cityId);

            var result = _mapper.Map<IEnumerable<PointOfInterestDto>>(pointOfInterest);
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
            var pointOfInterest = _mapper.Map<PointOfInterestDto>(pointEntity);
           
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
 
            if (!_cityInfoRepository.CityExists(cityId))
            {
                return NotFound();
            }


            var pointOfInterestDto = _mapper.Map<Entities.PointOfInterest>(pointOfInterestForCreatingDTO);

            _cityInfoRepository.InsertPointOfInterest(cityId, pointOfInterestDto);

            _cityInfoRepository.Save();

            var cretedPointOfInterestToReturn = _mapper.Map<Models.PointOfInterestDto>(pointOfInterestDto);

            return CreatedAtRoute("GetPointsOfInterests", new { cityId, id = cretedPointOfInterestToReturn.Id }, cretedPointOfInterestToReturn);

        }

        [HttpPut("{Id}")]
        public IActionResult UpdatePointOfInterest(int cityId, int id, [FromBody] PointOfInterestForCreatingDto pointOfInterestForCreatingDTO)
        {
                 
            if(!_cityInfoRepository.CityExists(cityId))
            {
                return NotFound();
            }
            var pointOfInterestEntity = _cityInfoRepository.GetPointOfInterestForCity(cityId, id);
            if(pointOfInterestEntity == null)
            {
                return NotFound();
            }

            _mapper.Map(pointOfInterestForCreatingDTO, pointOfInterestEntity);

            _cityInfoRepository.UpdatePointOfInterest(cityId, pointOfInterestEntity);

            _cityInfoRepository.Save();

            return NoContent();

        }

        [HttpPatch("{id}")]
        public IActionResult PartiallyUpdatePointOfInterest(int cityId, 
            int id, [FromBody] JsonPatchDocument<PointOfInterestForUpdatingDto> patchdoc)
        {
            
            if (!_cityInfoRepository.CityExists(cityId))
            {
                return NotFound();
            }
            var pointOfInterestEntity = _cityInfoRepository.GetPointOfInterestForCity(cityId, id);
            if(pointOfInterestEntity == null)
            {
                return NotFound();
            }

            var pointOfInterestToPatch = _mapper.Map<PointOfInterestForUpdatingDto>(pointOfInterestEntity);

            patchdoc.ApplyTo(pointOfInterestToPatch, ModelState);
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
           if(!TryValidateModel(pointOfInterestToPatch))
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(pointOfInterestToPatch, pointOfInterestEntity);
            _cityInfoRepository.UpdatePointOfInterest(cityId, pointOfInterestEntity);
            _cityInfoRepository.Save();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePointOfInterest(int cityId, int id)
        {
           if(!_cityInfoRepository.CityExists(cityId))
            {
                return NotFound();
            }

            var pointOfInterestEntity = _cityInfoRepository.GetPointOfInterestForCity(cityId, id);
            if(pointOfInterestEntity ==null)
            {
                return NotFound();
            }
            _cityInfoRepository.DetelePointOfInterest(pointOfInterestEntity);

            _cityInfoRepository.Save();



            _mailService.SendMail("point deleted", "your point of interest has been deleted");

            return NoContent(); 
        }
    }
}
