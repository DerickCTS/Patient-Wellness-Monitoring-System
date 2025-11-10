namespace Patient_Monitoring.DTOs.Authentication
{
    public class AuthResponseDto
    {
        public string message { get; set; } = null!;
        public string? token { get; set; }
        public string? refreshToken { get; set; }
        public int? id { get; set; }
    }
}
