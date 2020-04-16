using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Taxi_04.Models
{
    public class Person
    {
        public int ID { get; set; }

        [Required, StringLength(160, MinimumLength = 2)]
        public string Name { get; set; }

        [Required, StringLength(160, MinimumLength = 2)]
        public string Surname { get; set; }

        public string Fullname
        {
            get
            {
                return string.Format("{0} {1}", Surname, Name);
            }
        }

    }
}