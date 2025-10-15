using Microsoft.EntityFrameworkCore;
using Patient_Monitoring.Data;
using Patient_Monitoring.Models;

namespace Patient_Monitoring.Services
{
    public class SchedulingService
    {
        private readonly PatientMonitoringDbContext _context;

        public SchedulingService(PatientMonitoringDbContext context)
        {
            _context = context;
        }

        // Main method to run periodically (e.g., daily or weekly)
        public async Task GenerateUpcomingSlots(int daysInAdvance = 60)
        {
            var doctors = await _context.Doctors.ToListAsync();
            var startDate = DateTime.Today;
            var endDate = startDate.AddDays(daysInAdvance);

            foreach (var doctor in doctors)
            {
                await GenerateSlotsForDoctor(doctor.DoctorId, startDate, endDate);
            }
        }

        private async Task GenerateSlotsForDoctor(string doctorId, DateTime start, DateTime end)
        {
            var availabilities = await _context.DoctorAvailabilities
                                                .Where(da => da.DoctorId == doctorId)
                                                .ToListAsync();

            var timeOff = await _context.DoctorTimeOffs
                                         .Where(dto => dto.DoctorId == doctorId)
                                         .ToListAsync();

            // Loop through each day (date) in the range
            for (var date = start; date <= end; date = date.AddDays(1))
            {
                // Convert current day to a string (e.g., "Monday") to match availability table
                var dayOfWeekString = date.DayOfWeek.ToString();

                // Find the availability rule for this specific day
                var dayRule = availabilities.FirstOrDefault(da => da.DayOfWeek == dayOfWeekString);

                if (dayRule != null)
                {
                    // Break the availability rule into 30-minute slots (example duration)
                    var currentSlotTime = dayRule.StartTime;
                    var slotDuration = TimeSpan.FromMinutes(30);

                    while (currentSlotTime + slotDuration <= dayRule.EndTime)
                    {
                        var slotStartDateTime = date.Date.Add(currentSlotTime);
                        var slotEndDateTime = date.Date.Add(currentSlotTime + slotDuration);

                        // Check if this specific slot is blocked by DoctorTimeOff
                        bool isBlocked = timeOff.Any(dto =>
                            slotStartDateTime < dto.EndDateTime && slotEndDateTime > dto.StartDateTime);

                        // Check if the slot already exists (to prevent duplicates if job runs twice)
                        bool exists = await _context.AppointmentSlots.AnyAsync(s =>
                            s.DoctorId == doctorId && s.StartDateTime == slotStartDateTime);

                        if (!isBlocked && !exists)
                        {
                            _context.AppointmentSlots.Add(new AppointmentSlot
                            {
                                DoctorId = doctorId,
                                StartDateTime = slotStartDateTime,
                                EndDateTime = slotEndDateTime,
                                IsBooked = false
                            });
                        }

                        currentSlotTime = currentSlotTime.Add(slotDuration);
                    }
                }
            }
            await _context.SaveChangesAsync();
        }
    }
}
