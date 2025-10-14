namespace Patient_Monitoring.DTOs
{
    public class PendingApprovalDto
    {
        public string AppointmentID { get; set; }
        public string PatientID { get; set; }
        public string PatientName { get; set; }
        public int PatientAge { get; set; } // Calculated
        public string PatientGender { get; set; }
        public DateTime AppointmentDateTime { get; set; }
        public string ChiefComplaint { get; set; }
        public DateTime RequestedOn { get; set; }
    }
}
