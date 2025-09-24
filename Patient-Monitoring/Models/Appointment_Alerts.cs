namespace Patient_Monitoring.Models
{
    public class Appointment_Alerts
    {
        public required string AlertID { get; set; } // Primary Key
        public required string AppointmentID { get; set; } // Foreign Key to Appointments

        /*public required string AlertType { get; set; } // e.g., "Reminder", "Follow-up", "Cancellation"*/
    }
}
