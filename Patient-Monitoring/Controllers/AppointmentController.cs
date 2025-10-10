using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Patient_Monitoring.Data;
using Patient_Monitoring.Models;
using Patient_Monitoring.DTOs;
using System.Linq;

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

        // ------------------------------------------------------------------
        // GET /api/appointments/slots/{doctorId}
        // PATIENT'S VIEW: Retrieve all available slots for a specific doctor
        // ------------------------------------------------------------------
        [HttpGet("slots/{doctorId}")]
        public async Task<IActionResult> GetAvailableSlots(string doctorId)
        {
            if (string.IsNullOrWhiteSpace(doctorId))
            {
                return BadRequest("DoctorID is required.");
            }

            var availableSlots = await _context.AppointmentSlots
                .Where(s => s.DoctorID == doctorId && s.IsBooked == false)
                .OrderBy(s => s.StartDateTime)
                .Select(s => new
                {
                    s.SlotID,
                    s.StartDateTime,
                    s.EndDateTime
                })
                .ToListAsync();

            if (!availableSlots.Any())
            {
                return NotFound($"No available slots found for Doctor {doctorId}.");
            }

            return Ok(availableSlots);
        }

        // ------------------------------------------------------------------
        // POST /api/appointments/book
        // PATIENT'S ACTION: Book an available slot
        // ------------------------------------------------------------------
        [HttpPost("book")]
        public async Task<IActionResult> BookSlot([FromBody] AppointmentBookingDto bookingDto)
        {
            // 1. Find and validate the slot
            var slot = await _context.AppointmentSlots
                .FirstOrDefaultAsync(s => s.SlotID == bookingDto.SlotID && s.DoctorID == bookingDto.DoctorID);

            if (slot == null || slot.IsBooked)
            {
                return BadRequest("The selected slot is invalid or already booked.");
            }

            // Begin Transaction
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // 2. Mark the slot as booked/reserved
                slot.IsBooked = true;
                _context.AppointmentSlots.Update(slot);

                // 3. Create the new Appointment record (Status defaults to "Pending Approval")
                var appointment = new Appointment
                {
                    AppointmentID = Guid.NewGuid().ToString(), // Generate a unique ID
                    PatientID = bookingDto.PatientID,
                    DoctorID = bookingDto.DoctorID,
                    SlotID = slot.SlotID,
                    Appointment_Date_Time = slot.StartDateTime, // Use slot time
                    Reason = bookingDto.Reason,
                    Status = "Pending Approval", // Default Status
                    Notes = null // Leave notes empty for now
                };

                _context.Appointments.Add(appointment);

                // 4. (SIMULATION) Log alert for the doctor
                _context.Appointment_Alerts.Add(new Appointment_Alert
                {
                    AlertID = Guid.NewGuid().ToString(),
                    AppointmentID = appointment.AppointmentID,
                    // In a real app, you'd add more details here
                });

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return CreatedAtAction(nameof(BookSlot), new { id = appointment.AppointmentID }, new { message = "Appointment requested successfully. Awaiting doctor approval." });
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, "An error occurred during booking.");
            }
        }

        // ------------------------------------------------------------------
        // GET /api/appointments/pending/{doctorId}
        // DOCTOR'S VIEW: Retrieve appointments awaiting approval
        // ------------------------------------------------------------------
        [HttpGet("pending/{doctorId}")]
        public async Task<IActionResult> GetPendingAppointments(string doctorId)
        {
            var pending = await _context.Appointments
                .Where(a => a.DoctorID == doctorId && a.Status == "Pending Approval")
                .Select(a => new
                {
                    a.AppointmentID,
                    a.PatientID,
                    a.Appointment_Date_Time,
                    a.Reason
                })
                .ToListAsync();

            return Ok(pending);
        }

        // ------------------------------------------------------------------
        // POST /api/appointments/status
        // DOCTOR'S ACTION: Approve or Reject a pending appointment
        // ------------------------------------------------------------------
        [HttpPost("status")]
        public async Task<IActionResult> UpdateStatus([FromBody] AppointmentStatusUpdateDto updateDto)
        {
            var appointment = await _context.Appointments
                .Include(a => a.AppointmentSlot) // Include the slot for rejection logic
                .FirstOrDefaultAsync(a => a.AppointmentID == updateDto.AppointmentID);

            if (appointment == null)
            {
                return NotFound("Appointment not found.");
            }

            if (appointment.Status != "Pending Approval")
            {
                return BadRequest($"Appointment is already {appointment.Status}. Cannot change status.");
            }

            // Begin Transaction
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                if (updateDto.Action.Equals("Approve", StringComparison.OrdinalIgnoreCase))
                {
                    appointment.Status = "Confirmed";
                }
                else if (updateDto.Action.Equals("Reject", StringComparison.OrdinalIgnoreCase))
                {
                    appointment.Status = "Rejected";
                    appointment.RejectionReason = updateDto.RejectionReason;

                    // Crucial step: Free up the slot so the patient can re-book
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

                // (SIMULATION) Trigger patient notification alert
                // ... logic to update AppointmentAlerts table or external system ...

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
    }
}