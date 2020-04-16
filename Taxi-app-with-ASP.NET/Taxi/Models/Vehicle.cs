using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Taxi_04.Models
{
    public class Vehicle
    {
        public int ID { get; set; }

        [Required]
        public string Brand { get; set; }

        public string Model { get; set; }

        [Required]
        [Range(1, 12)]
        [Display(Name ="Max number of person")]
        public int MaxPerson { get; set; }

        [Required]
        [Display(Name = "Price per km")]
        public float PricePerKm { get; set; }

        public int DriverId { get; set; }
        [Display(Name = "Driver")]
        public Driver Driver { get; set; }

        public string Fullname
        {
            get
            {
                return string.Format("{0} {1}", Brand, Model);
            }
        }
    }
}