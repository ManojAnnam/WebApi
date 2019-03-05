using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace CityInfo.API.Controllers
{
    [Route("api/cities")]
    public class PointsofInterestController : Controller
    {
        private ILogger<PointsofInterestController> _logger;
        private IMailingService _mailingService;
        public PointsofInterestController(ILogger<PointsofInterestController> logger, IMailingService mailingService)
        {
            _logger = logger;
            _mailingService = mailingService;
        }
        [HttpGet("{id}/pointsofInterest")]
        public IActionResult GetPointsofInterest(int id)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.ID == id);
            if (city == null)
            {

                return NotFound();
            }
            return Ok(city.PointsofInterest);
        }
        [HttpGet("{id}/pointsofInterest/{pid}", Name = "GetPointofInterest")]
        public IActionResult GetPointofInterestofCity(int id, int pid)
        {
            try
            {
                var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.ID == id);
                if (city == null)
                {
                    _logger.LogInformation($"The city with id {id} is not found");
                    return NotFound();
                }
                var pointOfInterest = city.PointsofInterest.FirstOrDefault(f => f.ID == pid);
                if (pointOfInterest == null)
                {
                    return NotFound();
                }
                return Ok(pointOfInterest);
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

            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.ID == cityId);
            if (city == null)
            {
                return BadRequest();
            }

            var maxPointofInterestId = CitiesDataStore.Current.Cities.SelectMany(c => c.PointsofInterest).Max(p => p.ID);

            var finalPointofInterest = new PointofInterestDto
            {
                ID = ++maxPointofInterestId,
                Name = pointofInterestForCreationDto.Name,
                Description = pointofInterestForCreationDto.Description
            };
            city.PointsofInterest.Add(finalPointofInterest);
            return CreatedAtRoute("GetPointofInterest", new { id = cityId, pid = finalPointofInterest.ID }, finalPointofInterest);
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

            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.ID == cityId);
            if (city == null)
            {
                return BadRequest();
            }

            var pointOfInterestFromStore = city.PointsofInterest.FirstOrDefault(p => p.ID == id);

            if (pointOfInterestFromStore == null)
            {
                return BadRequest();
            }

            //Updation
            pointOfInterestFromStore.Name = pointofInterestforUpdateDto.Name;
            pointOfInterestFromStore.Description = pointofInterestforUpdateDto.Description;

            return NoContent();
        }

        [HttpPatch("{cityId}/pointsofInterest/{id}")]
        public IActionResult partialUpdation(int cityId, int id,
            [FromBody] JsonPatchDocument<PointofInterestforUpdateDto> jsonPatch)
        {
            if (jsonPatch == null)
            {
                return BadRequest();
            }

            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.ID == cityId);

            if (city == null)
            {
                return BadRequest();
            }

            var pointOfInterestFromStore = city.PointsofInterest.FirstOrDefault(p => p.ID == id);

            if (pointOfInterestFromStore == null)
            {
                return BadRequest();
            }

            var pointOfInterestToPatch = new PointofInterestforUpdateDto
            {
                Description = pointOfInterestFromStore.Description,
                Name = pointOfInterestFromStore.Name
            };

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



            pointOfInterestFromStore.Name = pointOfInterestToPatch.Name;
            pointOfInterestFromStore.Description = pointOfInterestToPatch.Description;

            return NoContent();
        }

        [HttpDelete("{cityId}/pointsofInterest/{id}")]
        public IActionResult DeletePointofInterest(int cityId, int id)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.ID == cityId);
            if (city == null)
            {
                return BadRequest();
            }

            var pointOfInterestFromStore = city.PointsofInterest.FirstOrDefault(p => p.ID == id);

            if (pointOfInterestFromStore == null)
            {
                return BadRequest();
            }

            city.PointsofInterest.Remove(pointOfInterestFromStore);
            _mailingService.send(pointOfInterestFromStore.Name, "is Deleted");

            return NoContent();
        }
    }
}
