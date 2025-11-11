namespace Patient_Monitoring.DTOs.Authentication
{
    public class AuthResponseDto
    {
        public string message { get; set; } = null!;
        public int? userId { get; set; }
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public string? specialization { get; set; }
        public string? imageUrl { get; set; }
    }
}
