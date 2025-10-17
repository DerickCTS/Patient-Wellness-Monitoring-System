using Microsoft.EntityFrameworkCore.Storage;
using Patient_Monitoring.Models;

public interface IDiagnosisRepository
{
    Task<List<Appointment>> GetTodaysAppointmentsForDoctorAsync(int doctorId);
    Task<Appointment?> GetAppointmentWithPatientDetailsAsync(int appointmentId);
    Task<List<Disease>> GetAllDiseasesAsync();
    Task AddDiagnosesAsync(IEnumerable<Diagnosis> diagnoses);
    Task AddPrescriptionsAsync(IEnumerable<Prescription> prescriptions);
    Task<bool> SaveChangesAsync();
    Task<IDbContextTransaction> BeginTransactionAsync();
}