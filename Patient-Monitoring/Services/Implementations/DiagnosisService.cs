using Patient_Monitoring.Models;

public class DiagnosisService : IDiagnosisService
{
    private readonly IDiagnosisRepository _diagnosisRepository;

    public DiagnosisService(IDiagnosisRepository diagnosisRepository)
    {
        _diagnosisRepository = diagnosisRepository;
    }

    // Implementation for GetTodaysAppointmentsAsync, GetPatientDiagnosisDetailsAsync, etc.
    // ...

    public async Task<bool> SaveDiagnosisAndPrescriptionsAsync(string appointmentId, string doctorId, SaveDiagnosisDto data)
    {
        var appointment = await _diagnosisRepository.GetAppointmentWithPatientDetailsAsync(appointmentId);
        if (appointment == null) return false;

        var newDiagnoses = data.Diagnoses.Select(d => new Diagnosis
        {
            DiagnosisId = Guid.NewGuid().ToString(),
            AppointmentId = appointmentId,
            PatientId = appointment.PatientId,
            DiseaseId = d.DiseaseId,
            Description = d.Description
        }).ToList();

        var newPrescriptions = data.Prescriptions.Select(p => new Prescription
        {
            PrescriptionId = Guid.NewGuid().ToString(),
            PatientId = appointment.PatientId,
            PrescribingDoctorId = doctorId,
            AppointmentId = appointmentId, // Linking the prescription to this appointment
            MedicationName = p.MedicationName,
            Dosage = p.Dosage,
            StartDate = p.StartDate,
            EndDate = p.EndDate,
            Instructions = p.Instructions,
            MedicationSchedules = p.Schedules.Select(s => new MedicationSchedule
            {
                ScheduleId = Guid.NewGuid().ToString(),
                TimeOfDay = s.TimeOfDay,
                Quantity = s.Quantity
            }).ToList()
        }).ToList();

        // Use a transaction to ensure all data is saved, or none is.
        await using var transaction = await _diagnosisRepository.BeginTransactionAsync();
        try
        {
            if (newDiagnoses.Any()) await _diagnosisRepository.AddDiagnosesAsync(newDiagnoses);
            if (newPrescriptions.Any()) await _diagnosisRepository.AddPrescriptionsAsync(newPrescriptions);

            await _diagnosisRepository.SaveChangesAsync();
            await transaction.CommitAsync();
            return true;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            return false;
        }
    }
}