using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Taxi_04.Models
{
    public class Passenger : Person
    {
        [Display(Name = "List of all rides")]
        public virtual ICollection<Ride> Rides { get; set; }
    }
}