using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Entities
{
    public class PointofInterest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
