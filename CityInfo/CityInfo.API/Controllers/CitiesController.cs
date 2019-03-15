using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Controllers
{
    [Route("api/cities")]
    public class CitiesController : Controller
    {
        private ICityInfoRepository _cityInfoRepository;
        public CitiesController(ICityInfoRepository cityInfoRepository)
        {
            _cityInfoRepository = cityInfoRepository;
        }
        [HttpGet()]
        public IActionResult GetCities()
        {
            //return Ok(CitiesDataStore.Current.Cities);
            var cities = _cityInfoRepository.GetCities();
            List<CityWithoutPointsofInterestsDto> cityWithoutPointsofInterestsDtos = new List<CityWithoutPointsofInterestsDto>();
            foreach(var city in cities)
            {
                cityWithoutPointsofInterestsDtos.Add(new CityWithoutPointsofInterestsDto
                {
                    ID = city.ID,
                    Name = city.Name,
                    Description = city.Description
                });

            }
            return Ok(cityWithoutPointsofInterestsDtos);
        }

        [HttpGet("{id}")]
        public IActionResult GetCity(int id,bool includePointsOfInterest = false)
        {
            try
            {
                var city = _cityInfoRepository.GetCity(id, includePointsOfInterest);
                if(city==null)
                {
                    return NotFound();
                }
                if(includePointsOfInterest)
                {
                    var cityDto = new CityDto()
                    {
                        ID = city.ID,
                        Name = city.Name,
                        Description = city.Description

                    };

                    foreach (var poi in city.PointsofInterest)
                    {
                        cityDto.PointsofInterest.Add(
                            new PointofInterestDto
                            {
                                ID = poi.ID,
                                Name = poi.Name,
                                Description = poi.Description
                                
                            });
                    }
                    return Ok(cityDto);
                }

                //Without Points of Interest
                CityWithoutPointsofInterestsDto cityWithoutPointsofInterestsDto= new CityWithoutPointsofInterestsDto()
                    {
                    ID = city.ID,
                    Name = city.Name,
                    Description = city.Description
                };
                return Ok(cityWithoutPointsofInterestsDto);
            }
            catch(Exception e)
            {
                return null;
            }
          
            //var cityToReturn = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.ID == id);
            //if(cityToReturn == null)
            //{
            //    return NotFound();
            //}

            //return Ok(cityToReturn);
        }
    }
}
