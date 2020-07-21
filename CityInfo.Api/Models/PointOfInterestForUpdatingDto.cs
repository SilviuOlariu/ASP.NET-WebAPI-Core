using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.Api.Models
{
    public class PointOfInterestForUpdatingDto
    {
        [Required(ErrorMessage = "you should provide a description")]
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(256)]
        public string Description { get; set; }
    }
}
