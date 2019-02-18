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
        [HttpGet("{id}/pointsofInterest/{pid}")]
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
    }
}
