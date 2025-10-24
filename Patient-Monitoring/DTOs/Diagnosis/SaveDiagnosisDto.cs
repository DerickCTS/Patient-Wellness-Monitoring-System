public class SaveDiagnosisDto
{
    public List<NewDiagnosisDto> Diagnoses { get; set; }
    public List<NewPrescriptionDto> Prescriptions { get; set; }
}

public class NewDiagnosisDto
{
    public int DiseaseId { get; set; }
    public string Description { get; set; }
}

public class NewPrescriptionDto
{
    public string MedicationName { get; set; }
    public string Dosage { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? Instructions { get; set; }
    public List<NewMedicationScheduleDto> Schedules { get; set; }
}

public class NewMedicationScheduleDto
{
    public string TimeOfDay { get; set; }
    public float Quantity { get; set; }
}