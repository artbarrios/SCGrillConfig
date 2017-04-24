using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SCGrillConfig.Models
{
    public class BuildTaskGrillConfgurations
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Build  Task")]
        public int BuildTaskId { get; set; }
        [ForeignKey("BuildTaskId")]
        [JsonIgnore]
        public virtual BuildTask BuildTask { get; set; }

        [Required]
        [Display(Name = "Grill  Confguration")]
        public int GrillConfgurationId { get; set; }
        [ForeignKey("GrillConfgurationId")]
        [JsonIgnore]
        public virtual GrillConfguration GrillConfguration { get; set; }

    } // public class BuildTaskGrillConfgurations
}
