using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SCGrillConfig.Models
{
    public class GrillConfguration
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Name")]
        [DataType(DataType.Text)]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "The {0} field must have a minimum of {2} and a maximum of {1} characters.")]
        public string Name { get; set; }

        [Display(Name = "Main Burner Count")]
        public int MainBurnerCount { get; set; }

        [Display(Name = "Infrared Burner Count")]
        [DataType(DataType.Text)]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "The {0} field must have a minimum of {2} and a maximum of {1} characters.")]
        public string InfraredBurnerCount { get; set; }

        [Required]
        [Display(Name = "Grill  Type")]
        public int GrillTypeId { get; set; }
        [ForeignKey("GrillTypeId")]
        [JsonIgnore]
        public virtual GrillType GrillType { get; set; }

        [Required]
        [Display(Name = "Fuel")]
        public int FuelId { get; set; }
        [ForeignKey("FuelId")]
        [JsonIgnore]
        public virtual Fuel Fuel { get; set; }

        [Required]
        [Display(Name = "Side  Burner  Type")]
        public int SideBurnerTypeId { get; set; }
        [ForeignKey("SideBurnerTypeId")]
        [JsonIgnore]
        public virtual SideBurnerType SideBurnerType { get; set; }

        [Required]
        [Display(Name = "Grill  Size")]
        public int GrillSizeId { get; set; }
        [ForeignKey("GrillSizeId")]
        [JsonIgnore]
        public virtual GrillSize GrillSize { get; set; }

        [Required]
        [Display(Name = "Material")]
        public int MaterialId { get; set; }
        [ForeignKey("MaterialId")]
        [JsonIgnore]
        public virtual Material Material { get; set; }

        [Required]
        [Display(Name = "Color")]
        public int ColorId { get; set; }
        [ForeignKey("ColorId")]
        [JsonIgnore]
        public virtual Color Color { get; set; }

        [Display(Name = "Flowchart Diagram Data")]
        [DataType(DataType.Text)]
        [StringLength(4000, MinimumLength = 1, ErrorMessage = "The {0} field must have a minimum of {2} and a maximum of {1} characters.")]
        public string BuildTaskFlowchartDiagramData { get; set; }

        [JsonIgnore]
        public virtual List<BuildTask> BuildTasks { get; set; }

    } // public class GrillConfguration
}
