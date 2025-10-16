using Patient_Monitoring.DTOs;
using Patient_Monitoring.Models;

namespace Patient_Monitoring.Repository.Interfaces
{
    public interface IPatientDoctorMappingRepository
    {
        /// <summary>
        /// Gets the total count of patients currently unmapped to any doctor.
        /// </summary>
        Task<int> GetUnmappedPatientsCountAsync();

        /// <summary>
        /// Retrieves a summary of all doctors, including their current patient load, for the Dashboard.
        /// </summary>
        Task<IEnumerable<DoctorAssignmentSummaryDTO>> GetDoctorAssignmentSummariesAsync();

        /// <summary>
        /// Gets the current number of patients assigned to a specific doctor.
        /// </summary>
        Task<int> GetPatientCountForDoctorAsync(int doctorId);

        /// <summary>
        /// Retrieves an existing mapping entry for a given patient ID, if one exists.
        /// </summary>
        Task<PatientDoctorMapper?> GetMappingByPatientIdAsync(int patientId);

        /// <summary>
        /// Creates a new patient-doctor mapping entry.
        /// </summary>
        Task<PatientDoctorMapper?> CreateMappingAsync(PatientDoctorMapper newMapping);

        /// <summary>
        /// Deletes the mapping entry associated with a specific patient ID.
        /// </summary>
        Task<bool> DeleteMappingByPatientIdAsync(int patientId);
    }
}
