using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CityInfo.API.Services
{
    public class CityInfoRepository : ICityInfoRepository
    {
        private CityInfoContext _cityInfoContext;
        public CityInfoRepository(CityInfoContext cityInfoContext)
        {
            _cityInfoContext = cityInfoContext;
        }

        public void AddPointofInterest(int cityId, PointofInterest pointofInterest)
        {
            var city = GetCity(cityId, false);
            city.PointsofInterest.Add(pointofInterest);
        }

        public void DeletePointofInterest(PointofInterest pointOfInterestFromEntity)
        {
            _cityInfoContext.PointsofInterests.Remove(pointOfInterestFromEntity);
        }

        public IEnumerable<City> GetCities()
        {
            return _cityInfoContext.Cities.OrderBy(c => c.Name).ToList();
        }

        public City GetCity(int cityId, bool includePointsOfInterest)
        {
            if (includePointsOfInterest)
            {
                return _cityInfoContext.Cities.Include(c => c.PointsofInterest).
                    Where(c => c.ID == cityId).FirstOrDefault();
            }
             return _cityInfoContext.Cities.Where(c => c.ID == cityId).FirstOrDefault();
        }

        public PointofInterest GetPointofInterest(int cityId, int pointofInterest)
        {
            return _cityInfoContext.PointsofInterests.Where(p => (p.CityID == cityId) && (p.ID == pointofInterest)).FirstOrDefault();
        }

        public IEnumerable<PointofInterest> GetPointofInterests(int cityId)
        {
            return _cityInfoContext.PointsofInterests.Where(p => p.CityID == cityId).ToList();
        }

        public bool IfCityExists(int cityId)
        {
            return _cityInfoContext.Cities.Any(c => c.ID == cityId);
        }

        public bool Save()
        {
            return (_cityInfoContext.SaveChanges() >= 0);
        }
    }
}
