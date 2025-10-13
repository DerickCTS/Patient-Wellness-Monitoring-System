namespace Patient_Monitoring.Enums
{
    public enum UserType
    {
        Patient = 1,
        Doctor = 2,
    }

    public enum AppointmentStatus
    {
        Pending,
        Accepted,
        Rejected,
        Cancelled
    }
    public enum NotificationType
    {
        AppointmentReminder,
        MedicationReminder,
        WellnessTip,
        AppointmentRequest,
        AppointmentResponse
    }
}
