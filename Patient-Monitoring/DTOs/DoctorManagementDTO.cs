public class DoctorManagementDTO
{
    public int DoctorId { get; set; }
    // Remove: public string DoctorIDCode { get; set; }
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public required string Specialization { get; set; }
    public int PatientsAssignedCount { get; set; }
    public int MaxCapacity { get; set; }
}