using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Patient_Monitoring.Data;
using Patient_Monitoring.Models;
using Patient_Monitoring.DTOs; // Ensure all DTOs are here
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Patient_Monitoring.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly PatientMonitoringDbContext _context;

        public AppointmentController(PatientMonitoringDbContext context)
        {
            _context = context;
        }

        // ====================================================================
        // HELPER METHOD
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

        // ====================================================================
        // 1. PATIENT ENDPOINTS
        // ====================================================================

        // GET /api/appointment/specializations
        // UI: Booking Pop-up Step 1 (Dropdown)
        [HttpGet("specializations")]
        public async Task<ActionResult<IEnumerable<string>>> GetSpecializations()
        {
            var specializations = await _context.Doctor_Details
                                                .Select(d => d.Specialization)
                                                .Distinct()
                                                .ToListAsync();

            if (!specializations.Any())
            {
                return NotFound("No specializations found.");
            }
            return Ok(specializations);
        }

        // GET /api/appointment/doctors/slots/specialization/{specialization}
        // UI: Booking Pop-up Step 2 (Doctor List) - Sorted by Experience
        [HttpGet("doctors/slots/specialization/{specialization}")]
        public async Task<ActionResult<IEnumerable<DoctorExperienceDto>>> GetDoctorsAndSlots(string specialization)
        {
            var doctors = await _context.Doctor_Details
                .Where(d => d.Specialization == specialization)
                .ToListAsync();

            if (!doctors.Any())
            {
                return NotFound($"No doctors found for specialization: {specialization}");
            }

            var doctorSlotsList = new List<DoctorExperienceDto>();
            var cutoffDate = DateTime.Today.AddDays(14);

            foreach (var doctor in doctors)
            {
                var experienceYears = CalculateExperienceYears(doctor.DoctorSince);

                var availableSlots = await _context.AppointmentSlots
                    .Where(s => s.DoctorID == doctor.DoctorId &&
                                s.IsBooked == false &&
                                s.StartDateTime >= DateTime.Now &&
                                s.StartDateTime <= cutoffDate)
                    .OrderBy(s => s.StartDateTime)
                    .Take(6)
                    .Select(s => new AvailableSlotDto
                    {
                        SlotID = s.SlotID,
                        StartDateTime = s.StartDateTime
                    })
                    .ToListAsync();

                if (availableSlots.Any())
                {
                    doctorSlotsList.Add(new DoctorExperienceDto
                    {
                        DoctorID = doctor.DoctorId,
                        Name = $"Dr. {doctor.FirstName} {doctor.LastName}",
                        Specialization = doctor.Specialization,
                        Education = doctor.Education,
                        ExperienceYears = experienceYears,
                        AvailableSlots = availableSlots
                    });
                }
            }

            return Ok(doctorSlotsList.OrderByDescending(d => d.ExperienceYears));
        }

        // POST /api/appointment/book
        [HttpPost("book")]
        public async Task<IActionResult> BookSlot([FromBody] AppointmentBookingDto bookingDto)
        {
            // Note: Updated logic to use AppointmentSlotID from AppointmentSlots for primary key
            var slot = await _context.AppointmentSlots
                .FirstOrDefaultAsync(s => s.SlotID == bookingDto.SlotID && s.DoctorID == bookingDto.DoctorID);

            if (slot == null || slot.IsBooked)
            {
                return BadRequest("The selected slot is invalid or already booked.");
            }

            // Get current timestamp for the RequestedOn field
            var requestedOn = DateTime.Now;

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // 1. Mark the slot as booked/reserved
                slot.IsBooked = true;
                _context.AppointmentSlots.Update(slot);

                // 2. Create the new Appointment record 
                var appointment = new Appointment
                {
                    AppointmentID = Guid.NewGuid().ToString(),
                    PatientID = bookingDto.PatientID,
                    DoctorID = bookingDto.DoctorID,
                    SlotID = slot.SlotID, // Use the SlotID from the slot table
                    AppointmentDate = slot.StartDateTime, // Use slot time
                    Reason = bookingDto.Reason,
                    Status = "Pending Approval",
                    RequestedOn = requestedOn, // Store the request time
                    Notes = null
                };

                _context.Appointments.Add(appointment);

                // 3. (SIMULATION) Log alert for the doctor
                // NOTE: Assuming your Appointment_Alert model exists and is linked correctly
                // var alert = new AppointmentAlert { ... }; 
                // _context.Appointment_Alerts.Add(alert);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return CreatedAtAction(nameof(BookSlot), new { id = appointment.AppointmentID }, new { message = "Appointment requested successfully. Awaiting doctor approval." });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                // Log the exception (ex)
                return StatusCode(500, "An error occurred during booking.");
            }
        }

        // GET /api/appointment/patient/{patientId}/status
        // UI: Patient Dashboard Tabs (Approved, Pending, Rejected)
        [HttpGet("patient/{patientId}/status")]
        public async Task<ActionResult<object>> GetPatientAppointmentsByStatus(string patientId)
        {
            var appointments = await _context.Appointments
                .Where(a => a.PatientID == patientId && a.AppointmentDate >= DateTime.Today) // Only show future/today's appointments
                .Join(_context.Doctor_Details,
                      a => a.DoctorID,
                      d => d.DoctorId,
                      (a, d) => new
                      {
                          a.AppointmentID,
                          a.AppointmentDate,
                          a.Reason,
                          a.Status,
                          a.RejectionReason,
                          DoctorName = $"Dr. {d.FirstName} {d.LastName}",
                          Specialization = d.Specialization
                      })
                .OrderBy(a => a.AppointmentDate)
                .ToListAsync();

            var response = new
            {
                Approved = appointments.Where(a => a.Status == "Confirmed").ToList(),
                Pending = appointments.Where(a => a.Status == "Pending Approval").ToList(),
                Rejected = appointments.Where(a => a.Status == "Rejected").ToList(),
            };

            return Ok(response);
        }

        // GET /api/appointment/patient/{patientId}/history
        // UI: Patient Dashboard Past Appointments Table
        [HttpGet("patient/{patientId}/history")]
        public async Task<ActionResult<IEnumerable<PastAppointmentDto>>> GetPastAppointments(string patientId)
        {
            var history = await _context.Appointments
                .Where(a => a.PatientID == patientId &&
                            (a.Status == "Confirmed" || a.Status == "Completed") &&
                            a.AppointmentDate < DateTime.Today)
                .Join(_context.Doctor_Details,
                      a => a.DoctorID,
                      d => d.DoctorId,
                      (a, d) => new PastAppointmentDto
                      {
                          AppointmentDate = a.AppointmentDate.Date,
                          AppointmentTime = a.AppointmentDate.TimeOfDay,
                          DoctorName = $"Dr. {d.FirstName} {d.LastName}",
                          Specialization = d.Specialization,
                          Reason = a.Reason,
                          Status = "Completed"
                      })
                .OrderByDescending(h => h.AppointmentDate)
                .ToListAsync();

            return Ok(history);
        }

        // ====================================================================
        // 2. DOCTOR ENDPOINTS
        // ====================================================================

        // GET /api/appointment/doctor/{doctorId}/metrics
        // UI: Doctor Dashboard Top Cards (Pending, Today's, Available Counts)
        [HttpGet("doctor/{doctorId}/metrics")]
        public async Task<ActionResult<object>> GetDoctorMetrics(string doctorId)
        {
            var today = DateTime.Today;

            var pendingCount = await _context.Appointments
                .CountAsync(a => a.DoctorID == doctorId && a.Status == "Pending Approval");

            var todayCount = await _context.Appointments
                .CountAsync(a => a.DoctorID == doctorId &&
                                 a.AppointmentDate.Date == today &&
                                 a.Status == "Confirmed");

            var availableCount = await _context.AppointmentSlots
                .CountAsync(s => s.DoctorID == doctorId &&
                                 s.StartDateTime >= DateTime.Now &&
                                 s.IsBooked == false);

            return Ok(new
            {
                PendingApprovals = pendingCount,
                TodaysAppointments = todayCount,
                AvailableSlots = availableCount
            });
        }

        // GET /api/appointment/doctor/{doctorId}/pending
        // UI: Doctor Dashboard Pending Approvals List
        [HttpGet("doctor/{doctorId}/pending")]
        public async Task<ActionResult<IEnumerable<PendingApprovalDto>>> GetPendingApprovals(string doctorId)
        {
            var pendingList = await _context.Appointments
                .Where(a => a.DoctorID == doctorId && a.Status == "Pending Approval")
                .Join(_context.Patient_Details,
                      a => a.PatientID,
                      p => p.PatientID,
                      (a, p) => new PendingApprovalDto
                      {
                          AppointmentID = a.AppointmentID,
                          PatientID = a.PatientID,
                          PatientName = $"{p.FirstName} {p.LastName}",
                          PatientAge = DateTime.Today.Year - p.DateOfBirth.Year, // Assumes Patient model has DateOfBirth
                          PatientGender = p.Gender,
                          AppointmentDateTime = a.AppointmentDate,
                          ChiefComplaint = a.Reason,
                          RequestedOn = a.RequestedOn  // Assumes Appointment model has RequestedOn
                      })
                .OrderBy(p => p.AppointmentDateTime)
                .ToListAsync();

            return Ok(pendingList);
        }

        // GET /api/appointment/doctor/{doctorId}/schedule?date=yyyy-MM-dd
        // UI: Doctor Dashboard Today's Schedule (detailed view)
        [HttpGet("doctor/{doctorId}/schedule")]
        public async Task<ActionResult<IEnumerable<DoctorSlotViewDto>>> GetDoctorSchedule(string doctorId, [FromQuery] DateTime date)
        {
            // 1. Get all slots for the specified date
            var slots = await _context.AppointmentSlots
                .Where(s => s.DoctorID == doctorId && s.StartDateTime.Date == date.Date)
                .OrderBy(s => s.StartDateTime)
                .ToListAsync();

            // 2. Get all confirmed appointments for this date
            var confirmedAppointments = await _context.Appointments
                .Where(a => a.DoctorID == doctorId &&
                            a.AppointmentDate.Date == date.Date &&
                            (a.Status == "Confirmed" || a.Status == "Pending Approval")) // Include pending for full view
                .Join(_context.Patient_Details,
                      a => a.PatientID,
                      p => p.PatientID,
                      (a, p) => new { Appointment = a, PatientName = $"{p.FirstName} {p.LastName}" })
                .ToDictionaryAsync(x => x.Appointment.AppointmentDate.TimeOfDay, x => x);

            // 3. Build the response list
            var schedule = new List<DoctorSlotViewDto>();
            foreach (var slot in slots)
            {
                var timeOfDay = slot.StartDateTime.TimeOfDay;

                // Determine status based on slot booked status and appointment existence
                string status = "Available";
                string patientName = null;
                string appointmentId = null;

                if (confirmedAppointments.TryGetValue(timeOfDay, out var appointmentDetails))
                {
                    status = appointmentDetails.Appointment.Status == "Confirmed" ? "Booked" : "Pending";
                    patientName = appointmentDetails.PatientName;
                    appointmentId = appointmentDetails.Appointment.AppointmentID;
                }

                schedule.Add(new DoctorSlotViewDto
                {
                    Time = timeOfDay,
                    Status = status,
                    PatientName = patientName,
                    AppointmentID = appointmentId
                });
            }

            return Ok(schedule);
        }

        // GET /api/appointment/doctor/{doctorId}/weekly-summary
        // UI: Doctor Dashboard Upcoming Week Schedule Overview
        [HttpGet("doctor/{doctorId}/weekly-summary")]
        public async Task<ActionResult<object>> GetWeeklyScheduleSummary(string doctorId)
        {
            var startDate = DateTime.Today;
            var endDate = startDate.AddDays(7);

            var allSlots = await _context.AppointmentSlots
                .Where(s => s.DoctorID == doctorId && s.StartDateTime >= startDate && s.StartDateTime < endDate)
                .ToListAsync();

            var allAppointments = await _context.Appointments
                .Where(a => a.DoctorID == doctorId && a.AppointmentDate >= startDate && a.AppointmentDate < endDate && a.Status == "Confirmed")
                .ToListAsync();

            var summary = allSlots
                .GroupBy(s => s.StartDateTime.Date)
                .OrderBy(g => g.Key)
                .Select(g => new
                {
                    Date = g.Key.ToString("MMMM dd, yyyy"),
                    DayOfWeek = g.Key.DayOfWeek.ToString(),
                    Booked = g.Count(s => allAppointments.Any(a => a.SlotID == s.SlotID)), // More accurate check
                    Available = g.Count(s => !s.IsBooked),
                    TotalSlots = g.Count()
                })
                .ToList();

            return Ok(summary);
        }

        // POST /api/appointment/doctor/manage-slots/manual
        // UI: Doctor Dashboard Manage Slots Pop-up (Manually create slots)
        // Requires ManualSlotCreationDto to be created
        [HttpPost("doctor/manage-slots/manual")]
        public async Task<IActionResult> ManuallyCreateSlots([FromBody] ManualSlotCreationDto model)
        {
            const int slotDurationMinutes = 30; // Assuming 30-minute slots

            var startDateTime = model.Date.Add(model.StartTime);
            var endDateTime = model.Date.Add(model.EndTime);
            var currentTime = startDateTime;

            var newSlots = new List<AppointmentSlot>();

            while (currentTime < endDateTime)
            {
                var slotEnd = currentTime.AddMinutes(slotDurationMinutes);

                // Check if the time slot is valid (end time not exceeded)
                if (slotEnd > endDateTime) break;

                // Crucial: Check if the slot already exists 
                var exists = await _context.AppointmentSlots
                    .AnyAsync(s => s.DoctorID == model.DoctorID &&
                                   s.StartDateTime == currentTime);

                if (!exists)
                {
                    newSlots.Add(new AppointmentSlot
                    {
                        DoctorID = model.DoctorID,
                        StartDateTime = currentTime,
                        EndDateTime = slotEnd,
                        IsBooked = false
                    });
                }
                currentTime = slotEnd;
            }

            if (newSlots.Any())
            {
                _context.AppointmentSlots.AddRange(newSlots);
                await _context.SaveChangesAsync();
            }

            return Ok(new { message = $"{newSlots.Count} new slot(s) created successfully for {model.Date.ToShortDateString()}." });
        }


        // ------------------------------------------------------------------
        // POST /api/appointment/status - DOCTOR'S ACTION: Approve or Reject
        // ------------------------------------------------------------------
        [HttpPost("status")]
        public async Task<IActionResult> UpdateStatus([FromBody] AppointmentStatusUpdateDto updateDto)
        {
            var appointment = await _context.Appointments
                .Include(a => a.AppointmentSlot)
                .FirstOrDefaultAsync(a => a.AppointmentID == updateDto.AppointmentID);

            if (appointment == null)
            {
                return NotFound("Appointment not found.");
            }

            if (appointment.Status != "Pending Approval")
            {
                return BadRequest($"Appointment is already {appointment.Status}. Cannot change status.");
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
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
                        // Validation required by UI (Image_4cb499.png)
                        return BadRequest("Rejection reason is required.");
                    }

                    appointment.Status = "Rejected";
                    appointment.RejectionReason = updateDto.RejectionReason;

                    // Crucial step: Free up the slot 
                    if (appointment.AppointmentSlot != null)
                    {
                        appointment.AppointmentSlot.IsBooked = false;
                        _context.AppointmentSlots.Update(appointment.AppointmentSlot);
                    }
                }
                else
                {
                    return BadRequest("Invalid action specified. Must be 'Approve' or 'Reject'.");
                }

                _context.Appointments.Update(appointment);

                // ... alert logic ...

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(new { message = $"Appointment {appointment.AppointmentID} successfully set to {appointment.Status}." });
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, "An error occurred while updating the appointment status.");
            }
        }

        // OLD ENDPOINT: No longer necessary as functionality is covered by GetDoctorsAndSlots
        // Retain for now if still used elsewhere, but marked for deprecation.
        [HttpGet("slots/{doctorId}")]
        public async Task<IActionResult> GetAvailableSlots(string doctorId)
        {
            var availableSlots = await _context.AppointmentSlots
                .Where(s => s.DoctorID == doctorId && s.IsBooked == false)
                .OrderBy(s => s.StartDateTime)
                .Select(s => new { s.SlotID, s.StartDateTime, s.EndDateTime })
                .ToListAsync();

            if (!availableSlots.Any())
            {
                return NotFound($"No available slots found for Doctor {doctorId}.");
            }

            return Ok(availableSlots);
        }

        // OLD ENDPOINT: Replaced by GetPendingApprovals which includes patient details
        [HttpGet("pending/{doctorId}")]
        public async Task<IActionResult> GetPendingAppointments(string doctorId)
        {
            return StatusCode(410, "This endpoint is deprecated. Use GET /api/appointment/doctor/{doctorId}/pending instead.");
        }
    }
}