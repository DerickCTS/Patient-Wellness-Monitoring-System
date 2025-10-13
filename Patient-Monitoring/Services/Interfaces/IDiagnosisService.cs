public interface IDiagnosisService
{
    Task<List<TodaysAppointmentCardDto>> GetTodaysAppointmentsAsync(string doctorId);
    Task<PatientDiagnosisDetailDto?> GetPatientDiagnosisDetailsAsync(string appointmentId);
    Task<List<DiseaseDto>> GetAllDiseasesAsync();
    Task<bool> SaveDiagnosisAndPrescriptionsAsync(string appointmentId, string doctorId, SaveDiagnosisDto data);
}