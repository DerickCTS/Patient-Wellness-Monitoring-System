using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Patient_Monitoring.Data;
using Patient_Monitoring.Models;

public class DiagnosisRepository : IDiagnosisRepository
{
    private readonly PatientMonitoringDbContext _context;

    public DiagnosisRepository(PatientMonitoringDbContext context)
    {
        _context = context;
    }



    public async Task<List<Appointment>> GetTodaysAppointmentsForDoctorAsync(int doctorId)
    {
        return await _context.Appointments
            .Include(a => a.Patient)
            .Include(a => a.AppointmentSlot) // Eager load the slot to get the EndTime
            .Where(a => a.DoctorId == doctorId && a.AppointmentDate.Date == DateTime.Today)
            .OrderBy(a => a.AppointmentDate)
            .ToListAsync();
    }

    public async Task<Appointment?> GetAppointmentWithPatientDetailsAsync(int appointmentId)
    {
        return await _context.Appointments
            .FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);
    }

    public async Task<List<Disease>> GetAllDiseasesAsync()
    {
        return await _context.Diseases.OrderBy(d => d.DiseaseName).ToListAsync();
    }

    public async Task AddDiagnosesAsync(IEnumerable<Diagnosis> diagnoses)
    {
        await _context.Diagnoses.AddRangeAsync(diagnoses);
    }

    public async Task AddPrescriptionsAsync(IEnumerable<Prescription> prescriptions)
    {
        await _context.Prescriptions.AddRangeAsync(prescriptions);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await _context.Database.BeginTransactionAsync();
    }
}