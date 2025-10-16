public class PatientManagementDTO
{
    public int PatientId { get; set; }
    // Remove: public string PatientIDCode { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string AssignedDoctorName { get; set; }
}