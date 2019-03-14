using CityInfo.API.Entities;
using System.Collections.Generic;

namespace CityInfo.API.Services
{
    public interface ICityInfoRepository
    {
        IEnumerable<City> GetCities();
        City GetCity(int cityId, bool includePointsOfInterest);
        IEnumerable<PointofInterest> GetPointofInterests(int cityId);
        PointofInterest GetPointofInterest(int cityId, int pointofInterest);
    }
}
