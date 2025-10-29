public class DashboardDto
{
    public double WellnessScore { get; set; }
    public int CurrentStreak { get; set; }
    public int BestStreak { get; set; }
    public string HealthTipOfTheDay { get; set; }
    public int TasksTodayCompleted { get; set; }
    public int TasksTodayTotal { get; set; }
    public int TasksThisWeekCompleted { get; set; }
    public int TasksThisWeekTotal { get; set; }
    public List<ActivityCalendarDay> ActivityCalendar { get; set; }
}

public class ActivityCalendarDay
{
    public DateTime Date { get; set; }
    public int CompletionLevel { get; set; } // e.g., 0-4 scale for color intensity
}