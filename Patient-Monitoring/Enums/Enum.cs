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
        Medication = 1,
        Appointment = 2,
        WellnessActivity = 3,
        MedicineReminder = 4,
        AppointmentReminder = 5,
        DailyScheduleSummary = 6
    }


}