using System.ComponentModel.DataAnnotations;

namespace Patient_Monitoring.Models
{
    public class Disease
    {
        [Required] public required string DiseaseId { get; set; } // Primary Key
        [Required] public required string DiseaseName { get; set; }
        public string? Description { get; set; }
    }
}
