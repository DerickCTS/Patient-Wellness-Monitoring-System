
using Patient_Monitoring.DTOs.DoctorDashboard;
using Patient_Monitoring.DTOs.WellnessPlan;
using Patient_Monitoring.Models;

using Patient_Monitoring.Repositories.Interfaces;
using Patient_Monitoring.Services.Interfaces;
using System.Runtime.CompilerServices;

namespace Patient_Monitoring.Services.Implementations
{
    public class DoctorService : IDoctorService

    {
        private readonly IPatientRepository _patientRepository;
        private readonly IDoctorRepository _doctorRepo;

        public DoctorService(IPatientRepository patientRepository, IDoctorRepository doctorRepository)
        {
            _patientRepository = patientRepository;
            _doctorRepo = doctorRepository;
        }

        #region Search Patient By Name or Id
        public new async Task<List<PatientSearchItemDto>> SearchPatientsAsync(string? patientName, int? patientId)
        {

            var patients = await _patientRepository.SearchPatientsByIdOrNameAsync(patientName, patientId);


            var searchResults = patients.Select(p => new PatientSearchItemDto
            {
                PatientId = p.PatientId,
                FirstName = p.FirstName,
                LastName = p.LastName ?? string.Empty,
                Gender = p.Gender,
                ProfileImage = p.ProfileImage ?? string.Empty,
                BloodGroup = p.BloodGroup
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



            var wellnessPlans = patient.AssignedPlans.Select(
                plan => new WellnessPlanDetailDto
                {
                    PlanId = plan.PlanId,
                    PlanName = plan.AssignedWellnessPlan.PlanName,
                    Goal = plan.AssignedWellnessPlan.Goal,
                    Status = plan.IsActive ? "Active" : "Inactive",
                    PlanImage = plan.AssignedWellnessPlan.ImageUrl,
                    Category = plan.AssignedWellnessPlan.Category,
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
                EmergencyContactNumber = patient.EmergencyContactNumber,
                EmergencyContactName = patient.EmergencyContactName,
                PersonalizedDoctor = personalizedDoctorName,
                Diagnoses = diagnoses.Select(item => new DiagnosisDetailDto
                {
                    DiagnosisId = item.DiagnosisId,
                    DiagnosisDate = item.DiagnosedDate,
                    DiseaseName = item.DiseaseName,
                    DoctorName = item.DiagnosedBy
                }).ToList(),
                WellnessPlans = wellnessPlans
            };
            return dto;

        }

        #endregion


        #region Assign New Wellness Plan
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
        #endregion


        #region Get Diagnosis Details
        public async Task<DiagnosisDetailsDto> GetDiagnosisDetailsAsync(int diagnosisId)
        {
            var diagnosis = await _patientRepository.GetDiagnosisWithDiseaseAsync(diagnosisId);

            if (diagnosis == null)
            {
                throw new KeyNotFoundException($"Diagnosis with ID {diagnosisId} not found.");
            }

            return new DiagnosisDetailsDto
            {
                DiagnosedBy = diagnosis.Appointment.Doctor.FirstName + diagnosis.Appointment.Doctor.LastName,
                DiagnosisDescription = diagnosis.DiagnosisDescription,
                DiseaseDescription = diagnosis.Disease.DiseaseDescription,
                MedicationDetails = diagnosis.Appointment.Prescriptions.Select(
                    prescription => new MedicationDetailDto
                    {
                        MedicationName = prescription.MedicationName,
                        Dosage = prescription.Dosage,
                        StartDate = prescription.StartDate,
                        EndDate = prescription.EndDate,
                    }).ToList()
            };
        }
        #endregion

        #region DoctorDashboard
        public async Task<DoctorDashboardDto> GetDashboardStatsAsync(int doctorId)
        {
            var stats = new DashboardStatsDto
            {
                TotalPatients = await _doctorRepo.GetTotalPatientsCountAsync(doctorId),
                ActivePrescriptions = await _doctorRepo.GetTotalActivePrescriptionsCountAsync(doctorId),
                TotalWellnessPlans = await _doctorRepo.GetTotalWellnessPlansCountAsync(doctorId),
                TodaysAppointments = await _doctorRepo.GetTodaysAppointmentsCountAsync(doctorId),
                CompletedToday = await _doctorRepo.GetAppointmentsCompletedTodayAsync(doctorId)
            };

            DateTime startOfWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);

            var weeklyAppointmentSummary = await _doctorRepo.GetWeeklyAppointmentSummaryAsync(doctorId, startOfWeek);
            
            var groupedSummary = weeklyAppointmentSummary
                .GroupBy(a => a.AppointmentDate.DayOfWeek)
                .Select(g => new WeeklyAppointmentSummaryItem
                {
                    Day = g.Key.ToString(),
                    Approved = g.Count(a => a.Status == "Approved"),
                    Rejected = g.Count(a => a.Status == "Rejected")
                }).ToList();

            var weeklySchedule = (await _doctorRepo.GetWeeklyScheduleAsync(doctorId))
                .Select(da => new WeeklyScheduleItem
                {
                    Day = da.DayOfWeek.ToString(),
                    StartTime = da.StartTime,
                    EndTime = da.EndTime
                }).ToList();

            var todaysAppointments = await _doctorRepo.GetTodaysAppointmentsAsync(doctorId);
            var todaysAppointmentSummary = todaysAppointments
                .Select(a => new TodaysAppointmentSummaryItem
                {
                    ProfileUrl = a.Patient.ProfileImage ?? string.Empty,
                    PatientName = $"{a.Patient.FirstName} {a.Patient.LastName}",
                    PatientAge = DateTime.Today.Year - a.Patient.DateOfBirth.Year,
                    AppointmentStartTime = a.AppointmentDate.TimeOfDay,
                    Reason = a.Reason,
                }).ToList();

            var recentPrescriptions = await _doctorRepo.GetRecentPrescriptionsAsync(doctorId);
            var recentPrescriptionSummary = recentPrescriptions
                .Select(p => new RecentPrescriptionSummaryItem
                {
                    MedicationName = p.MedicationName,
                    PatientName = $"{p.Patient.FirstName} {p.Patient.LastName}",
                    Dosage = p.Dosage,
                    Frequency = p.MedicationSchedules.Count,
                    MedicationStartDate = p.StartDate.ToString("yyyy-MM-dd"),
                    MedicationEndDate = p.EndDate.ToString("yyyy-MM-dd"),
                }).ToList();

            return new DoctorDashboardDto
            {
                Stats = stats,
                WeeklyAppointmentSummary = groupedSummary,
                WeeklyScheduleSummary = weeklySchedule,
                TodaysAppointment = todaysAppointmentSummary,
                RecentPrescriptions = recentPrescriptionSummary
            };
        }

        Task<DailyAppointmentSummaryDto> IDoctorService.GetAppointmentsCompletedTodayAsync(int doctorId, DateTime date)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<WeeklyAppointmentSummaryItem>> IDoctorService.GetWeeklyAppointmentDataAsync(int doctorId)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<RecentPrescriptionDto>> IDoctorService.GetRecentPrescriptionsAsync(int doctorId, int count)
        {
            throw new NotImplementedException();
        }

        Task<object> IDoctorService.GetPatientsBreakdownAsync(int doctorId)
        {
            throw new NotImplementedException();
        }

        Task<object> IDoctorService.GetPrescriptionsBreakdownAsync(int doctorId)
        {
            throw new NotImplementedException();
        }

        Task<object> IDoctorService.GetWellnessPlansBreakdownAsync(int doctorId)
        {
            throw new NotImplementedException();
        }

        Task<DoctorProfileDto> IDoctorService.GetDoctorProfileAsync(int doctorId)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}


