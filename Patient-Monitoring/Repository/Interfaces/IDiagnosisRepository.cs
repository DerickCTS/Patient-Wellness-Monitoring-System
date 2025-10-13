using Microsoft.EntityFrameworkCore.Storage;
using Patient_Monitoring.Models;

public interface IDiagnosisRepository
{
    Task<List<Appointment>> GetTodaysAppointmentsForDoctorAsync(string doctorId);
    Task<Appointment?> GetAppointmentWithPatientDetailsAsync(string appointmentId);
    Task<List<Disease>> GetAllDiseasesAsync();
    Task AddDiagnosesAsync(IEnumerable<Diagnosis> diagnoses);
    Task AddPrescriptionsAsync(IEnumerable<Prescription> prescriptions);
    Task<bool> SaveChangesAsync();
    Task<IDbContextTransaction> BeginTransactionAsync();
}