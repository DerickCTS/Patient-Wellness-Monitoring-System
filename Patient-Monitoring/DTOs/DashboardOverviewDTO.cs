namespace Patient_Monitoring.DTOs
{
    public class DashboardOverviewDTO
    {
        public int TotalPatients { get; set; }
        public int TotalDoctors { get; set; }
        public int TotalScheduledAppointments { get; set; }
        public int UnmappedPatients { get; set; }
        public required List<DoctorAssignmentSummaryDTO> DoctorAssignments { get; set; }
    }

    public class DoctorAssignmentSummaryDTO
    {
        public required int DoctorId { get; set; }
        public required string DoctorName { get; set; }
        public required string Specialization { get; set; }
        public int PatientsAssigned { get; set; }
        public int MaxCapacity { get; set; } = 10;
        public required List<PatientSummaryDTO> AssignedPatients { get; set; }
    }

    public class PatientSummaryDTO
    {
        public int PatientId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        // For the initials avatar
        public string Initials => $"{FirstName.FirstOrDefault()}{LastName.FirstOrDefault()}".ToUpper();
    }

    public class AppointmentDTO
    {
        public required int AppointmentId { get; set; }
        public required string DoctorName { get; set; }
        public required string PatientName { get; set; }
        public required string Specialization { get; set; }
        public DateOnly Date { get; set; }

        /// <summary>
        /// This single property combines the start and end times into a formatted string 
        /// (e.g., "09:30 - 10:30"), which is derived from the AppointmentSlot model 
        /// inside the AppointmentRepository.
        /// </summary>
        public required string TimeSlot { get; set; } // e.g., "09:00 - 09:30"
        public required string Status { get; set; } // "Completed" or "Pending"

    }

    public class PatientManagementDTO
    {
        public int PatientId { get; set; }
        //public string? PatientIDCode { get; set; } // e.g., P-1234
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string AssignedDoctorName { get; set; }
    }

    public class DoctorManagementDTO
    {
        public required int DoctorId { get; set; }
        //public string? DoctorIDCode { get; set; } // e.g., D-101
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public required string Specialization { get; set; }
        public int PatientsAssignedCount { get; set; }
        public int MaxCapacity { get; set; } = 10;
    }

    public class PatientDoctorMappingDTO
    {
        public int PatientId { get; set; }
        public required string PatientName { get; set; }
        public required string PatientEmail { get; set; }
        public required string PatientIDCode { get; set; }
        public int? AssignedDoctorId { get; set; }
        public required string AssignedDoctorName { get; set; }
        public string Initials => $"{PatientName.Split(' ')[0].FirstOrDefault()}{PatientName.Split(' ').Last().FirstOrDefault()}".ToUpper();
    }
}


