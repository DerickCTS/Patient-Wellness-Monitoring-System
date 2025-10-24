
using Patient_Monitoring.DTOs.WellnessPlan;
using Patient_Monitoring.Models;

using Patient_Monitoring.Repositories.Interfaces;
using Patient_Monitoring.Services.Interfaces;


namespace Patient_Monitoring.Services.Implementations
{
    public class DoctorService : IDoctorService

    {
        private readonly IPatientRepository _patientRepository;

        public DoctorService(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        #region Search Patient By Name or Id
        public new async Task<List<PatientSearchItemDto>> SearchPatientsAsync(string? patientName, int? patientId)
        {

            var patients = await _patientRepository.SearchPatientsByIdOrNameAsync(patientName, patientId);


            var searchResults = patients.Select(p => new PatientSearchItemDto
            {
                PatientId = p.PatientId,
                FirstName = p.FirstName,
                LastName = p.LastName,
                Gender = p.Gender
            }).ToList();

            return searchResults;
        }
        #endregion


        #region Retrieve Patient Details by Id
        public async Task<PatientFullDetailDto?> GetPatientDetailsAsync(int patientId)
        {
            // 1. Fetch Core Patient Data
            var patient = await _patientRepository.GetPatientByIdAsync(patientId);
            if (patient == null)
            {
                return null;
            }
            var doctor = patient.PersonalizedDoctorMapper?.Doctor;

            // Construct the full name string using the confirmed properties: FirstName and LastName
            string personalizedDoctorName = doctor != null
                ? $"{doctor.FirstName} {doctor.LastName}"
                : "Dr. Not Assigned";

            var diagnoses = patient.Diagnoses.Select(
                diagnosis => new
                {
                    diagnosis.Disease.DiseaseName,
                    DiagnosedBy = diagnosis.Appointment.Doctor.FirstName + diagnosis.Appointment.Doctor.LastName,
                    diagnosis.DiagnosisId,
                    DiagnosedDate = diagnosis.Appointment.AppointmentDate
                }).ToList();

            var medications = patient.Prescriptions.Select(p => new MedicationDetailDto
            {
                MedicationName = p.MedicationName,


                IsActive = p.EndDate >= DateTime.UtcNow,
                Dosage = p.Dosage,
                StartDate = p.StartDate,
                EndDate = p.EndDate
            }).ToList();

            var wellnessPlans = patient.AssignedPlans.Select(
                plan => new WellnessPlanDetailDto
                {
                    PlanName = plan.AssignedWellnessPlan.PlanName,
                    FrequencyCount = plan.FrequencyCount,
                    FrequencyUnit = plan.FrequencyUnit,
                    WellnessType = plan.AssignedWellnessPlan.CreatedByDoctor == null ? "General" : "Custom"
                }).ToList();


            var dto = new PatientFullDetailDto
            {
                // Patient Details (from Patient Model)
                PatientId = patient.PatientId,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                Email = patient.Email,
                Gender = patient.Gender,
                DateOfBirth = patient.DateOfBirth,
                BloodGroup = patient.BloodGroup,
                ContactNumber = patient.ContactNumber,
                Address = patient.Address,
                EmergencyContact = patient.EmergencyContactNumber,
                PersonalizedDoctor = personalizedDoctorName,
                Diagnoses = diagnoses.Select(item => new DiagnosisDetailDto
                {
                    DiagnosisId = item.DiagnosisId,
                    DiagnosisDate = item.DiagnosedDate,
                    DiseaseName = item.DiseaseName,
                    DoctorName = item.DiagnosedBy
                }).ToList(),
                Medications = medications,
                WellnessPlans = wellnessPlans
            };
            return dto;

        }

        #endregion
        

        // Implementations for GetWellnessPlanDetailsAsync would go here.
        public async Task AssignPlanAsync(AssignPlanRequest request)
        {
            // 1. Create the new AssignedWellnessPlan entity
            WellnessPlan newWellnessPlan = new WellnessPlan
            {
                PlanName = request.PlanName!,
                IsTemplate = false,
                Goal = request.Goal!,
                Category = request.Category!,
                // Using a default image URL
                ImageUrl = "https://defaultimage.url/plan.png",
                CreatedByDoctorId = request.DoctorId
            };

            await _patientRepository.AddWellnessPlanAsync(newWellnessPlan);

            // 2. Create the PatientPlan entity
            PatientPlanAssignment newPatientPlanAssignment = new PatientPlanAssignment
            {
                PatientId = request.PatientId,
                // Link to the newly created plan
                PlanId = newWellnessPlan.PlanId,
                AssignedByDoctorId = request.DoctorId,
                FrequencyCount = request.FrequencyCount,
                FrequencyUnit = request.FrequencyUnit!,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                IsActive = true,
            };

            await _patientRepository.AddPatientPlanAssignmentAsync(newPatientPlanAssignment);

            // 3. Create the list of AssignmentPlanDetail entities
            List<AssignmentPlanDetail> assignmentPlanDetails = new List<AssignmentPlanDetail>();
            int assignmentId = newPatientPlanAssignment.AssignmentId;

            // Add Description as a detail
            assignmentPlanDetails.Add(new AssignmentPlanDetail
            {
                AssignmentId = assignmentId,
                DetailType = "Description",
                Content = request.Description!,
            });

            // Add all Instructions
            if (request.Instruction != null)
            {
                request.Instruction.ForEach(instruction => assignmentPlanDetails.Add(new AssignmentPlanDetail
                {
                    AssignmentId = assignmentId,
                    DetailType = "Instruction",
                    Content = instruction,
                }));
            }

            // Add all Benefits
            if (request.Benefits != null)
            {
                request.Benefits.ForEach(benefit => assignmentPlanDetails.Add(new AssignmentPlanDetail
                {
                    AssignmentId = assignmentId,
                    DetailType = "Benefit",
                    Content = benefit,
                }));
            }

        }
        public async Task<DiagnosisDetailsDto> GetDiagnosisDetailsAsync(int diagnosisId)
        {
            // 1. Fetch data from the repository (uses .Include() to get Disease)
            var diagnosis = await _patientRepository.GetDiagnosisWithDiseaseAsync(diagnosisId);

            // 2. Handle 'Not Found' Case
            if (diagnosis == null)
            {
                throw new KeyNotFoundException($"Diagnosis with ID {diagnosisId} not found.");
            }

            // 3. Handle Data Integrity Check
            if (diagnosis.Disease == null)
            {
                throw new InvalidOperationException($"Diagnosis found (ID: {diagnosisId}), but the associated disease data is missing or corrupted.");
            }

            // 4. Map the entities to the simplified DTO
            return new DiagnosisDetailsDto
            {
                // Maps the three required fields:
              
                DiagnosisDescription = diagnosis.DiagnosisDescription,
               DiseaseDescription=diagnosis.Disease.DiseaseDescription,

                // DoctorName and DiagnosisDate are intentionally excluded
            };
        }
    }
}


