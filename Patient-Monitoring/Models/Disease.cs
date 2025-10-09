using System.ComponentModel.DataAnnotations;

namespace Patient_Monitoring.Models
{
    public class Disease
    {
        [Key]
        [Required]
        public required string DiseaseId { get; set; } // Primary Key


        [Required]
        [StringLength(100, ErrorMessage = "DiseaseName cannot exceed 100 characters.")]
        public required string DiseaseName { get; set; }


        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string? Description { get; set; }

        public ICollection<Diagnosis>? Diagnosis { get; set; }
    }
}
