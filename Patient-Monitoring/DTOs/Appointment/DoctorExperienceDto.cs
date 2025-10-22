using System.Collections.Generic;

namespace Patient_Monitoring.DTOs.Appointment
{
    public class DoctorExperienceDto
    {
        public int DoctorID { get; set; }
        public string Name { get; set; }
        public string Specialization { get; set; } // Added for clarity
        public int ExperienceYears { get; set; } // Calculated value
        public string Education { get; set; } // Added for Doctor credentials display
        public List<AvailableSlotDto> AvailableSlots { get; set; }
    }
}
