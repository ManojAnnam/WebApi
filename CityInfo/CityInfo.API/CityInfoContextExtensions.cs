using CityInfo.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API
{
    public static class CityInfoContextExtensions
    {
        public static void EnsureSeedDateForContext(this CityInfoContext cityInfoContext)
        {
            if(cityInfoContext.Cities.Any())
            {
                return;
            }
             var Cities = new List<City>()
            {
                new City()
                {
                  
                    Name = "Guntur",
                    Description = "My Home Town.",
                    PointsofInterest = new List<PointofInterest>()
                    {
                        new PointofInterest()
                        {
                          
                             Name = "SankarVilas",
                             Description = "Always traffic."
                        },
                        new PointofInterest()
                        {
                            
                             Name = "Lodge Center",
                             Description = "My College Bus Stop."
                        }

                    }
                },
                new City()
                {
                  
                    Name = "Hyderabad",
                    Description = "My Job Town.",
                    PointsofInterest = new List<PointofInterest>()
                    {
                        new PointofInterest()
                        {
                            
                             Name = "Hyderabad1",
                             Description = "My Job Town1."
                        },
                        new PointofInterest()
                        {
                            
                             Name = "Hyderabad2",
                             Description = "My Job Town2."
                        }

                    }
                },
                 new City()
                {
                   
                    Name = "NewYork",
                    Description = "My dream Town.",
                    PointsofInterest = new List<PointofInterest>()
                    {
                        new PointofInterest()
                        {
                            
                             Name = "Hyderabad1",
                             Description = "My Job Town1."
                        },
                        new PointofInterest()
                        {
                            
                             Name = "Hyderabad2",
                             Description = "My Job Town2."
                        }

                    }
                }
            };
            cityInfoContext.Cities.AddRange(Cities);
            cityInfoContext.SaveChanges();
        }
    }
}
