using Microsoft.AspNetCore.Mvc;
using Patient_Monitoring.DTOs;
using Patient_Monitoring.Models;
using Patient_Monitoring.Repositories.Interfaces;
using Patient_Monitoring.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Patient_Monitoring.DTOs.Appointment; // ADDED: Required for Regex in ID generation

namespace Patient_Monitoring.Services.Implementations
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _repository;

        public AppointmentService(IAppointmentRepository repository)
        {
            _repository = repository;
        }

        // ====================================================================
        // HELPER METHOD (Service Layer Business Logic)
        // ====================================================================

        /// <summary>
        /// Calculates the doctor's experience in full years based on the DoctorSince date.
        /// </summary>
        private int CalculateExperienceYears(DateTime doctorSince)
        {
            var today = DateTime.Today;
            int experience = today.Year - doctorSince.Year;

            // Adjust the age if the current date is before the doctor's work anniversary this year
            if (doctorSince.Date > today.AddYears(-experience))
            {
                experience--;
            }
            return experience;
        }

        /// <summary>
        /// Generates the next sequential Appointment ID (e.g., A100, A101, ...).
        /// Note: This is a simple implementation and requires a fully thread-safe 
        /// mechanism in a high-concurrency production environment.
        /// </summary>
        //private async Task<string> GenerateNextAppointmentId()
        //{
        //    const string prefix = "A";
        //    const int initialId = 100;

        //    // Get the highest existing ID in the required format from the repository
        //    string lastId = await _repository.GetLastSequentialAppointmentIdAsync();

        //    int nextNumber = initialId;

        //    if (!string.IsNullOrEmpty(lastId))
        //    {
        //        // Use regex to extract the number part from IDs like 'A109'
        //        var match = Regex.Match(lastId, @"^" + prefix + @"(\d+)$");

        //        if (match.Success && int.TryParse(match.Groups[1].Value, out int currentNumber))
        //        {
        //            nextNumber = currentNumber + 1;
        //        }
        //    }

        //    return prefix + nextNumber;
        //}

        // ====================================================================
        // 1. PATIENT FLOW
        // ====================================================================

        public async Task<ActionResult<IEnumerable<string>>> GetSpecializationsAsync()
        {
            var specializations = await _repository.GetDistinctSpecializationsAsync();

            if (!specializations.Any())
            {
                return new NotFoundObjectResult("No specializations found.");
            }
            return new OkObjectResult(specializations);
        }

        public async Task<ActionResult<IEnumerable<DoctorExperienceDto>>> GetDoctorsAndSlotsBySpecializationAsync(string specialization)
        {
            var doctors = await _repository.GetDoctorsBySpecializationAsync(specialization);

            if (!doctors.Any())
            {
                return new NotFoundObjectResult($"No doctors found for specialization: {specialization}");
            }

            var doctorSlotsList = new List<DoctorExperienceDto>();
            var cutoffDate = DateTime.Today.AddDays(14);

            foreach (var doctor in doctors)
            {
                var experienceYears = CalculateExperienceYears(doctor.DoctorSince);
                var availableSlots = await _repository.GetDoctorAvailableSlotsAsync(doctor.DoctorId, cutoffDate);

                if (availableSlots.Any())
                {
                    doctorSlotsList.Add(new DoctorExperienceDto
                    {
                        DoctorID = doctor.DoctorId,
                        Name = $"Dr. {doctor.FirstName} {doctor.LastName}",
                        Specialization = doctor.Specialization,
                        Education = doctor.Education,
                        ExperienceYears = experienceYears,
                        AvailableSlots = availableSlots.Select(s => new AvailableSlotDto
                        {
                            SlotID = s.SlotId,
                            StartDateTime = s.StartDateTime
                        }).ToList()
                    });
                }
            }

            return new OkObjectResult(doctorSlotsList.OrderByDescending(d => d.ExperienceYears));
        }

        public async Task<IActionResult> BookSlotAsync(AppointmentBookingDto bookingDto)
        {
            var slot = await _repository.GetSlotForBookingAsync(bookingDto.SlotId, bookingDto.DoctorId);

            if (slot == null || slot.IsBooked)
            {
                return new BadRequestObjectResult("The selected slot is invalid or already booked.");
            }

            var requestedOn = DateTime.Now;

            // --- FIX: Generate the next sequential ID ---
            //int newAppointmentId = await GenerateNextAppointmentId();

            using var transaction = await _repository.BeginTransactionAsync();
            try
            {
                // 1. Mark the slot as booked/reserved
                slot.IsBooked = true;
                _repository.UpdateAppointmentSlot(slot);

                // 2. Create the new Appointment record
                var appointment = new Appointment
                {
                    PatientId = bookingDto.PatientId,
                    DoctorId = bookingDto.DoctorId,
                    SlotId = slot.SlotId,
                    AppointmentDate = slot.StartDateTime,
                    Reason = bookingDto.Reason,
                    Status = "Pending Approval",
                    RequestedOn = requestedOn,
                };

                _repository.AddAppointment(appointment);

                // 3. Save changes and commit transaction
                await _repository.SaveChangesAsync();
                await transaction.CommitAsync();

                return new CreatedAtActionResult(nameof(BookSlotAsync), "Appointment", new { id = appointment.AppointmentId }, new { message = "Appointment requested successfully. Awaiting doctor approval." });
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                // In a real app, log the exception here
                return new StatusCodeResult(500);
            }
        }

        public async Task<ActionResult<object>> GetPatientAppointmentsByStatusAsync(int patientId)
        {
            var appointments = await _repository.GetPatientFutureAppointmentsAsync(patientId);

            var appointmentsDto = appointments.Select(a => new
            {
                a.AppointmentId,
                a.AppointmentDate,
                a.Reason,
                a.Status,
                a.RejectionReason,
                DoctorName = $"Dr. {a.Doctor?.FirstName} {a.Doctor?.LastName}", // Use null-conditional operator
                Specialization = a.Doctor?.Specialization
            }).ToList();

            var response = new
            {
                Approved = appointmentsDto.Where(a => a.Status == "Confirmed").ToList(),
                Pending = appointmentsDto.Where(a => a.Status == "Pending Approval").ToList(),
                Rejected = appointmentsDto.Where(a => a.Status == "Rejected").ToList(),
            };

            return new OkObjectResult(response);
        }

        public async Task<ActionResult<IEnumerable<PastAppointmentDto>>> GetPastAppointmentsAsync(int patientId)
        {
            var history = await _repository.GetPatientPastAppointmentsAsync(patientId);

            var historyDto = history.Select(a => new PastAppointmentDto
            {
                AppointmentDate = a.AppointmentDate.Date,
                AppointmentTime = a.AppointmentDate.TimeOfDay,
                DoctorName = $"Dr. {a.Doctor?.FirstName} {a.Doctor?.LastName}",
                Specialization = a.Doctor?.Specialization,
                Reason = a.Reason,
                Status = "Completed" // Simplified status for past history
            }).ToList();

            return new OkObjectResult(historyDto);
        }

        // ====================================================================
        // 2. DOCTOR FLOW
        // ====================================================================

        public async Task<ActionResult<object>> GetDoctorMetricsAsync(int doctorId)
        {
            var today = DateTime.Today;

            var pendingCountTask = await _repository.CountAppointmentsAsync(a => a.DoctorId == doctorId && a.Status == "Pending Approval");
            var todayCountTask = await _repository.CountAppointmentsAsync(a => a.DoctorId == doctorId && a.AppointmentDate.Date == today && a.Status == "Confirmed");
            var availableCountTask = await _repository.CountAppointmentSlotsAsync(s => s.DoctorId == doctorId && s.StartDateTime >= DateTime.Now && s.IsBooked == false);

            return new OkObjectResult(new
            {
                PendingApprovals = pendingCountTask,
                TodaysAppointments = todayCountTask,
                AvailableSlots = availableCountTask
            });
        }

        public async Task<ActionResult<IEnumerable<PendingApprovalDto>>> GetPendingApprovalsAsync(int doctorId)
        {
            var pendingAppointments = await _repository.GetPendingAppointmentsWithPatientDetailsAsync(doctorId);

            var pendingListDto = pendingAppointments.Select(a => new PendingApprovalDto
            {
                AppointmentId = a.AppointmentId,
                PatientId = a.PatientId,
                PatientName = $"{a.Patient?.FirstName} {a.Patient?.LastName}",
                PatientAge = a.Patient != null ? DateTime.Today.Year - a.Patient.DateOfBirth.Year : 0,
                PatientGender = a.Patient?.Gender,
                AppointmentDateTime = a.AppointmentDate,
                ChiefComplaint = a.Reason,
                RequestedOn = a.RequestedOn
            }).ToList();

            return new OkObjectResult(pendingListDto);
        }

        // 🎯 CRITICAL FIX: Changed repository method call and simplified mapping.
        public async Task<ActionResult<IEnumerable<DoctorSlotViewDto>>> GetDoctorScheduleAsync(int doctorId, DateTime date)
        {
            // 1. CALL THE NEW REPOSITORY METHOD THAT INCLUDES APPOINTMENT AND PATIENT
            var slots = await _repository.GetDoctorSlotsWithAppointmentDetailsAsync(doctorId, date);

            var schedule = new List<DoctorSlotViewDto>();

            foreach (var slot in slots)
            {
                var timeOfDay = slot.StartDateTime.TimeOfDay;
                var appointment = slot.Appointment; // This is loaded via Include, or null if unbooked.

                string status = "Available";
                string patientName = null;
                int appointmentId = 0;

                if (appointment != null)
                {
                    if (appointment.Status == "Confirmed") status = "Booked";
                    else if (appointment.Status == "Pending Approval") status = "Pending";
                    else if (appointment.Status == "Rejected") status = "Available"; // Slot is theoretically free if rejected

                    appointmentId = appointment.AppointmentId;

                    // 2. USE NULL-CONDITIONAL OPERATOR ON THE EAGERLY LOADED PATIENT OBJECT
                    if (appointment.Patient != null)
                    {
                        patientName = $"{appointment.Patient.FirstName} {appointment.Patient.LastName}";
                    }
                }

                schedule.Add(new DoctorSlotViewDto
                {
                    Time = timeOfDay,
                    DisplayTime = slot.StartDateTime.ToShortTimeString(), // Added DisplayTime for clarity
                    Status = status,
                    PatientName = patientName,
                    AppointmentId = appointmentId
                });
            }

            return new OkObjectResult(schedule);
        }

        public async Task<ActionResult<object>> GetWeeklyScheduleSummaryAsync(int doctorId)
        {
            var startDate = DateTime.Today;
            var endDate = startDate.AddDays(7);

            var allSlotsTask = await _repository.CountAppointmentSlotsAsync(s => s.DoctorId == doctorId && s.StartDateTime >= startDate && s.StartDateTime < endDate);
            var confirmedAppointmentsTask = await _repository.GetConfirmedAppointmentsForDoctor(doctorId, startDate, endDate);

           // await Task.WhenAll(allSlotsTask, confirmedAppointmentsTask);

            var allAppointments = confirmedAppointmentsTask;

            // FIX: Simplified logic to group the retrieved appointments by date.
            var summary = allAppointments
                .GroupBy(a => a.AppointmentDate.Date)
                .OrderBy(g => g.Key)
                .Select(g => new
                {
                    Date = g.Key.ToString("MMMM dd, yyyy"),
                    DayOfWeek = g.Key.DayOfWeek.ToString(),
                    // Counting all pending/confirmed appointments for the day
                    Booked = g.Count(),
                    // Cannot accurately calculate available/total slots without a dedicated repo method,
                    // so retaining minimal logic based on available data.
                    Available = 0,
                    TotalSlots = g.Count()
                })
                .ToList();

            return new OkObjectResult(summary);
        }

        public async Task<IActionResult> ManuallyCreateSlotsAsync(ManualSlotCreationDto model)
        {
            const int slotDurationMinutes = 30;
            var startDateTime = model.Date.Add(model.StartTime);
            var endDateTime = model.Date.Add(model.EndTime);
            var currentTime = startDateTime;
            var newSlots = new List<AppointmentSlot>();

            while (currentTime < endDateTime)
            {
                var slotEnd = currentTime.AddMinutes(slotDurationMinutes);
                if (slotEnd > endDateTime) break;

                // Crucial: Check if the slot already exists 
                var exists = await _repository.AppointmentSlotExistsAsync(model.DoctorId, currentTime);

                if (!exists)
                {
                    newSlots.Add(new AppointmentSlot
                    {
                        DoctorId = model.DoctorId,
                        StartDateTime = currentTime,
                        EndDateTime = slotEnd,
                        IsBooked = false
                    });
                }
                currentTime = slotEnd;
            }

            if (newSlots.Any())
            {
                _repository.AddAppointmentSlots(newSlots);
                await _repository.SaveChangesAsync();
            }

            return new OkObjectResult(new { message = $"{newSlots.Count} new slot(s) created successfully for {model.Date.ToShortDateString()}." });
        }

        public async Task<IActionResult> UpdateAppointmentStatusAsync(AppointmentStatusUpdateDto updateDto)
        {
            var appointment = await _repository.GetAppointmentByIdIncludeSlotAsync(updateDto.AppointmentId);

            if (appointment == null)
            {
                return new NotFoundObjectResult("Appointment not found.");
            }

            if (appointment.Status != "Pending Approval")
            {
                return new BadRequestObjectResult($"Appointment is already {appointment.Status}. Cannot change status.");
            }

            using var transaction = await _repository.BeginTransactionAsync();
            try
            {
                if (updateDto.Action.Equals("Approve", StringComparison.OrdinalIgnoreCase))
                {
                    appointment.Status = "Confirmed";
                }
                else if (updateDto.Action.Equals("Reject", StringComparison.OrdinalIgnoreCase))
                {
                    if (string.IsNullOrWhiteSpace(updateDto.RejectionReason))
                    {
                        return new BadRequestObjectResult("Rejection reason is required.");
                    }

                    appointment.Status = "Rejected";
                    appointment.RejectionReason = updateDto.RejectionReason;

                    // Crucial step: Free up the slot 
                    if (appointment.AppointmentSlot != null)
                    {
                        appointment.AppointmentSlot.IsBooked = false;
                        _repository.UpdateAppointmentSlot(appointment.AppointmentSlot);
                    }
                }
                else
                {
                    return new BadRequestObjectResult("Invalid action specified. Must be 'Approve' or 'Reject'.");
                }

                _repository.UpdateAppointment(appointment);
                await _repository.SaveChangesAsync();
                await transaction.CommitAsync();

                return new OkObjectResult(new { message = $"Appointment {appointment.AppointmentId} successfully set to {appointment.Status}." });
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return new StatusCodeResult(500);
            }
        }
    }
}