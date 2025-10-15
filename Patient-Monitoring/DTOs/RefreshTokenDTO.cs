using System.ComponentModel.DataAnnotations;

namespace Patient_Monitoring.DTOs
{
    public class RefreshTokenDTO
    {
        [Required(ErrorMessage = "Refresh Token is required.")]
        public string RefreshToken { get; set; } = null!;

        //[Required(ErrorMessage = "Client Id is required.")]
        //public string ClientId { get; set; } = null!;
    }
}
