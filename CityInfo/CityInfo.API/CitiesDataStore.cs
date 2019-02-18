using CityInfo.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API
{
    public class CitiesDataStore
    {
        public static CitiesDataStore Current { get; } = new CitiesDataStore();

        public List<CityDto> Cities { get; set; }

        public CitiesDataStore()
        {
            Cities = new List<CityDto>()
            {
                new CityDto()
                {
                    ID = 1,
                    Name = "Guntur",
                    Description = "My Home Town.",
                    PointsofInterest = new List<PointofInterestDto>()
                    {
                        new PointofInterestDto()
                        {
                             ID = 1,
                             Name = "SankarVilas",
                             Description = "Always traffic."
                        },
                        new PointofInterestDto()
                        {
                             ID = 2,
                             Name = "Lodge Center",
                             Description = "My College Bus Stop."
                        }

                    }
                },
                new CityDto()
                {
                    ID = 2,
                    Name = "Hyderabad",
                    Description = "My Job Town.",
                    PointsofInterest = new List<PointofInterestDto>()
                    {
                        new PointofInterestDto()
                        {
                             ID = 1,
                             Name = "Hyderabad1",
                             Description = "My Job Town1."
                        },
                        new PointofInterestDto()
                        {
                             ID = 2,
                             Name = "Hyderabad2",
                             Description = "My Job Town2."
                        }

                    }
                },
                 new CityDto()
                {
                    ID = 3,
                    Name = "NewYork",
                    Description = "My dream Town.",
                    PointsofInterest = new List<PointofInterestDto>()
                    {
                        new PointofInterestDto()
                        {
                             ID = 1,
                             Name = "Hyderabad1",
                             Description = "My Job Town1."
                        },
                        new PointofInterestDto()
                        {
                             ID = 2,
                             Name = "Hyderabad2",
                             Description = "My Job Town2."
                        }

                    }
                }
            };
        }
    }
}
