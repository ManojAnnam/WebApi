using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Controllers
{
    [Route("api/cities")]
    public class PointsofInterestController : Controller
    {
        [HttpGet("{id}/pointsofInterest")]
        public IActionResult GetPointsofInterest(int id)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.ID == id);
            if( city == null)
            {
                return NotFound();
            }
            return Ok(city.PointsofInterest);
        }
        [HttpGet("{id}/pointsofInterest/{pid}",Name = "GetPointofInterest")]
        public IActionResult GetPointofInterestofCity(int id,int pid)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.ID == id);
            if( city == null)
            {
                return NotFound();
            }
            var pointOfInterest = city.PointsofInterest.FirstOrDefault(f => f.ID == pid);
            if( pointOfInterest == null)
            {
                return NotFound();
            }
            return Ok(pointOfInterest);
        }
        [HttpPost("{cityId}/pointsofInterest")]
        public IActionResult CreatePointofInterest (int cityId,[FromBody]PointofInterestForCreationDto pointofInterestForCreationDto)
        {
            if (pointofInterestForCreationDto == null)
            {
                return BadRequest();
            }

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(pointofInterestForCreationDto.Name == pointofInterestForCreationDto.Description)
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
            return CreatedAtRoute("GetPointofInterest", new { id = cityId , pid = finalPointofInterest.ID}, finalPointofInterest);
        }
    }
}
