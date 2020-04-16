using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Taxi_04.Models
{
    public class City
    {
        public int ID { get; set; }

        [Required]
        [StringLength(160, MinimumLength = 2)]
        public string Name { get; set; }

        [Required]
        [RegularExpression(@"^[-+]?([1-8]?\d(\.\d+)?|90(\.0+)?),\s*[-+]?(180(\.0+)?|((1[0-7]\d)|([1-9]?\d))(\.\d+)?)$", ErrorMessage = "Not valid geo-location")]
        public string Location { get; set; }
    }
}