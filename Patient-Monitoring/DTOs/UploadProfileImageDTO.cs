using System.ComponentModel.DataAnnotations;

namespace Patient_Monitoring.DTOs
{
    public class UploadProfileImageDTO
    {
        [Required]
        public required string Base64Image { get; set; }
    }
}
