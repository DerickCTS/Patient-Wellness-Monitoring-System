public class PatientDiagnosisDetailDto
{
    public PatientDetail PatientInfo { get; set; }
    public List<ExistingDiagnosisDto> ExistingDiagnoses { get; set; }
    public List<ExistingPrescriptionDto> ExistingPrescriptions { get; set; }
}

public class PatientDetail
{
    public string Name { get; set; }
    public int PatientId { get; set; }
    public int Age { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Gender { get; set; }
    public string ContactNumber { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public string EmergencyContact { get; set; }
    public string ChiefComplaint { get; set; }
}

public class ExistingDiagnosisDto 
{ 
    public string DiseaseName { get; set; }
    public string Description { get; set; }
}
public class ExistingPrescriptionDto 
{ 
    public string MedicationName {  get; set; }
    public string Dosage {  get; set; }
}