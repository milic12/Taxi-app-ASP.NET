using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Taxi_04.Models
{
    public class Driver : Person
    {
        [Display(Name = "List of all rides")]
        public virtual ICollection<Ride> Rides { get; set; }

        [Display(Name="Owned Vehicles")]
        public virtual ICollection<Vehicle> OwnedVehicles { get; set; }

    }
}