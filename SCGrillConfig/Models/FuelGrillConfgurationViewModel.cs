using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SCGrillConfig.Models
{
    public class FuelGrillConfgurationViewModel
    {

        [Key]
        public int Id { get; set; }

        public int FuelId { get; set; }

        [Display(Name = "Name")]
        [DataType(DataType.Text)]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "The {0} field must have a minimum of {2} and a maximum of {1} characters.")]
        public string Fuel_Name { get; set; }

        public int GrillConfgurationId { get; set; }

        [Display(Name = "Name")]
        [DataType(DataType.Text)]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "The {0} field must have a minimum of {2} and a maximum of {1} characters.")]
        public string GrillConfguration_Name { get; set; }

    } // public class FuelGrillConfgurationViewModel
}
