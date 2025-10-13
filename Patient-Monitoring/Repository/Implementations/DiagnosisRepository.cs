using Microsoft.EntityFrameworkCore.Storage;
using Patient_Monitoring.Models;

public class DiagnosisRepository : IDiagnosisRepository
{
    private readonly ApplicationDbContext _context;

    public DiagnosisRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Appointment>> GetTodaysAppointmentsForDoctorAsync(string doctorId)
    {
        return await _context.Appointments
            .Include(a => a.Patient)
            .Where(a => a.DoctorId == doctorId && a.AppointmentDate.Date == DateTime.Today)
            .OrderBy(a => a.AppointmentDate)
            .ToListAsync();
    }

    public async Task<Appointment?> GetAppointmentWithPatientDetailsAsync(string appointmentId)
    {
        return await _context.Appointments
            .Include(a => a.Patient)
            // Include existing diagnoses and prescriptions for this appointment
            .Include(a => a.Diagnoses)
            .Include(a => a.Patient).ThenInclude(p => p.Medications.Where(m => m.AppointmentId == appointmentId))
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