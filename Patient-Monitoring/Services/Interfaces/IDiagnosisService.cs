public interface IDiagnosisService
{
    Task<List<TodaysAppointmentCardDto>> GetTodaysAppointmentsAsync(int doctorId);
    Task<PatientDiagnosisDetailDto?> GetPatientDiagnosisDetailsAsync(int appointmentId);
    Task<List<DiseaseDto>> GetAllDiseasesAsync();
    Task<bool> SaveDiagnosisAndPrescriptionsAsync(int appointmentId, int doctorId, SaveDiagnosisDto data);
}