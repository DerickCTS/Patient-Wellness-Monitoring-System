public class PatientDoctorMappingDTO
{
    public int PatientId { get; set; }
    public required string PatientName { get; set; }
    public required string PatientEmail { get; set; }
    // Remove: public string PatientIDCode { get; set; }
    public int? AssignedDoctorId { get; set; }
    public required string AssignedDoctorName { get; set; }
}