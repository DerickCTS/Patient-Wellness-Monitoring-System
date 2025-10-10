namespace Patient_Monitoring.DTOs
{
    public class AssignedDoctorDTO
    {
        public required string DoctorId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Specialization { get; set; }
        public required string ContactNumber { get; set; }
        public required string Email { get; set; }
        public string? Image { get; set; }
       
        public  required string Schedule { get; set; }
        public decimal Rating { get; set; }
        public required int Experience { get; set; }
        public required string Education { get; set; }
        public required List<string> Languages { get; set; }
        public int TotalPatients { get; set; }
        //public  string? NextAvailable { get; set; }
        public string? ConsultationFee { get; set; }
        public required DoctorRelationshipDTO Relationship { get; set; }
    }

    public class DoctorRelationshipDTO
    {
        public DateTime AssignedDate { get; set; }
        public required string Duration { get; set; }
        public int TotalVisits { get; set; }
        public DateTime LastVisit { get; set; }
        
    }
}
