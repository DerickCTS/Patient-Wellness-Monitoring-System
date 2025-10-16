namespace Patient_Monitoring.DTOs
{
    public class AdminLoginResultDTO
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public AdminResponseDTO? Response { get; set; }
    }
}
