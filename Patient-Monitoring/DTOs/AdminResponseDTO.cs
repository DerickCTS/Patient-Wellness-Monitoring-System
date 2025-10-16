namespace Patient_Monitoring.DTOs
{
    public class AdminResponseDTO
    {
        public int AdminId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string Token { get; set; } // The JWT
    }
}
