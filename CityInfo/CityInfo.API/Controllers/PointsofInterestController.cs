using AutoMapper;
using CityInfo.API.Entities;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CityInfo.API.Controllers
{
    [Route("api/cities")]
    public class PointsofInterestController : Controller
    {
        private ILogger<PointsofInterestController> _logger;
        private IMailingService _mailingService;
        private ICityInfoRepository _cityInfoRepository;
        public PointsofInterestController(ILogger<PointsofInterestController> logger, IMailingService mailingService, ICityInfoRepository cityInfoRepository)
        {
            _logger = logger;
            _mailingService = mailingService;
            _cityInfoRepository = cityInfoRepository;
        }
        [HttpGet("{id}/pointsofInterest")]
        public IActionResult GetPointsofInterest(int id)
        {
            //var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.ID == id);
            //if (city == null)
            //{

            //    return NotFound();
            //}
            //return Ok(city.PointsofInterest);

            var ifCityExists = _cityInfoRepository.IfCityExists(id);

            if(ifCityExists)
            {
                var poi = _cityInfoRepository.GetPointofInterests(id);
                var pointofInterestDtos = Mapper.Map<IEnumerable<PointofInterestDto>>(poi);
                //List<PointofInterestDto> pointofInterestDtos = new List<PointofInterestDto>();
                //foreach(var interest in poi)
                //{
                //    pointofInterestDtos.Add(
                //        new PointofInterestDto
                //        {
                //            ID = interest.ID,
                //            Name = interest.Name,
                //            Description = interest.Description
                //        });
                //}

                return Ok(pointofInterestDtos);

            }
            return NotFound();

        }
        [HttpGet("{id}/pointsofInterest/{pid}", Name = "GetPointofInterest")]
        public IActionResult GetPointofInterestofCity(int id, int pid)
        {
            try
            {
                var ifCityExists = _cityInfoRepository.IfCityExists(id);
                //var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.ID == id);
                if (!ifCityExists)
                {
                    _logger.LogInformation($"The city with id {id} is not found");
                    return NotFound();
                }
                var pointOfInterest = _cityInfoRepository.GetPointofInterest(id, pid);
                if (pointOfInterest == null)
                {
                    return NotFound();
                }

                PointofInterestDto pointofInterestDto = Mapper.Map<PointofInterestDto>(pointOfInterest);
                return Ok(pointofInterestDto);
            }
            catch (Exception ex)
            {
                _logger.LogCritical("I have thrown Exception", ex);
                return StatusCode(500, "A problem occured while Getting Point of Interest");
            }
        }
        [HttpPost("{cityId}/pointsofInterest")]
        public IActionResult CreatePointofInterest(int cityId, [FromBody]PointofInterestForCreationDto pointofInterestForCreationDto)
        {
            if (pointofInterestForCreationDto == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (pointofInterestForCreationDto.Name == pointofInterestForCreationDto.Description)
            {
                ModelState.AddModelError("Description", "Name and Description should not be the same dude");
                return BadRequest(ModelState);
            }

            var city = _cityInfoRepository.GetCity(cityId, false);
            if (city == null)
            {
                return BadRequest();
            }

            //var maxPointofInterestId = CitiesDataStore.Current.Cities.SelectMany(c => c.PointsofInterest).Max(p => p.ID);

            var finalPointofInterest = Mapper.Map<PointofInterest>(pointofInterestForCreationDto);
            _cityInfoRepository.AddPointofInterest(cityId, finalPointofInterest);
            if(!_cityInfoRepository.Save())
            {
                return StatusCode(500, "Not able to Add POI");
            }

            var finalPointofInterestDTO = Mapper.Map<PointofInterestForCreationDto>(finalPointofInterest);
            return CreatedAtRoute("GetPointofInterest", new { id = cityId, pid = finalPointofInterest.ID }, finalPointofInterestDTO);
        }

        [HttpPut("{cityid}/pointsofInterest/{id}")]
        public IActionResult UpdatePointofInterest(int cityId, int id,
            [FromBody]PointofInterestforUpdateDto pointofInterestforUpdateDto)
        {
            if (pointofInterestforUpdateDto == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.ID == cityId);
           
            if (!_cityInfoRepository.IfCityExists(cityId))
            {
                return BadRequest();
            }

            var pointOfInterestFromEntity = _cityInfoRepository.GetPointofInterest(cityId, id);
            if (pointOfInterestFromEntity == null)
            {
                return BadRequest();
            }

            Mapper.Map(pointofInterestforUpdateDto, pointOfInterestFromEntity);

            if(!_cityInfoRepository.Save())
            {
                return StatusCode(500, "Problem in Saving");
            }

           

            //Updation
            //pointOfInterestFromStore.Name = pointofInterestforUpdateDto.Name;
            //pointOfInterestFromStore.Description = pointofInterestforUpdateDto.Description;

            return NoContent();
        }

        [HttpPatch("{cityId}/pointsofInterest/{id}")]
        public IActionResult PartialUpdation(int cityId, int id,
            [FromBody] JsonPatchDocument<PointofInterestforUpdateDto> jsonPatch)
        {
            if (jsonPatch == null)
            {
                return BadRequest();
            }

           // var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.ID == cityId);

            if (!_cityInfoRepository.IfCityExists(cityId))
            {
                return BadRequest();
            }

            var pointOfInterestFromEntity = _cityInfoRepository.GetPointofInterest(cityId, id);

            if (pointOfInterestFromEntity == null)
            {
                return BadRequest();
            }

            var pointOfInterestToPatch = Mapper.Map<PointofInterestforUpdateDto>(pointOfInterestFromEntity);
            //var pointOfInterestToPatch = new PointofInterestforUpdateDto
            //{
            //    Description = pointOfInterestFromStore.Description,
            //    Name = pointOfInterestFromStore.Name
            //};

            jsonPatch.ApplyTo(pointOfInterestToPatch, ModelState);

            //Checking the patchDoc
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Checking the model
            TryValidateModel(pointOfInterestToPatch);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }



            Mapper.Map(pointOfInterestToPatch, pointOfInterestFromEntity);

            if(!_cityInfoRepository.Save())
            {
                return StatusCode(500, "Not Possible Dude");
            }

            return NoContent();
        }

        [HttpDelete("{cityId}/pointsofInterest/{id}")]
        public IActionResult DeletePointofInterest(int cityId, int id)
        {
           // var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.ID == cityId);
            if (!_cityInfoRepository.IfCityExists(cityId))
            {
                return BadRequest();
            }

            var pointOfInterestFromEntity = _cityInfoRepository.GetPointofInterest(cityId, id);

            if (pointOfInterestFromEntity == null)
            {
                return BadRequest();
            }

            _cityInfoRepository.DeletePointofInterest(pointOfInterestFromEntity);

            if(!_cityInfoRepository.Save())
            {
                return StatusCode(500, "Internal Server Error");
            }
            _mailingService.Send(pointOfInterestFromEntity.Name, "is Deleted");

            return NoContent();
        }
    }
}
