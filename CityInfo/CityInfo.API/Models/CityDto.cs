using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Models
{
    public class CityDto
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int NumberofPointsofInterests
        {
            get
            {
                return PointsofInterest.Count;
            }
        }

        public ICollection<PointofInterestDto> PointsofInterest { get; set; } = new List<PointofInterestDto>();
    }
}
