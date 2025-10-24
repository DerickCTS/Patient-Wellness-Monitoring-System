using Patient_Monitoring.DTOs.WellnessPlan;

public class ProgressService : IProgressService
{
    private readonly IProgressRepository _progressRepository;

    public ProgressService(IProgressRepository progressRepository)
    {
        _progressRepository = progressRepository;
    }

    #region Retrieving Assigned Plan Cards
    public async Task<List<AssignedPlanCardDto>> GetAssignedPlanCardsAsync(int patientId, string statusFilter, string categoryFilter, string dateFilter)
    {
        // Each card needs data from PatientPlanAssignement, WellnessPlan, DailyTaskLogs. All these tables are connected & hence all the data
        // can be accessed using PatientPlanAssignment table (acts as a central tables) & using its navigation properties.
        var assignments = await _progressRepository.GetActiveAssignmentsWithTasksAsync(patientId);
     
        var cards = new List<AssignedPlanCardDto>();

        foreach (var assignment in assignments)
        {
            // Retrieves cards either for the day/week/month as per dateFilter.
            var relevantTask = assignment.TaskLogs
                .OrderBy(t => t.DueDate)
                .FirstOrDefault();

            if (relevantTask == null) continue;

            cards.Add(new AssignedPlanCardDto
            {
                AssignmentId = assignment.AssignmentId,
                PlanName = assignment.AssignedWellnessPlan.PlanName,
                ImageUrl = assignment.AssignedWellnessPlan.ImageUrl,
                Category = assignment.AssignedWellnessPlan.Category,
                Status = relevantTask.Status,
                DueDate = relevantTask.DueDate,
                TaskLogId = relevantTask.LogId
            });
        }

        // APPLY FILTERS

        List<AssignedPlanCardDto> finalCards = new List<AssignedPlanCardDto>();

        
        DateTime today = DateTime.Now;
        DateTime startDate = today;

        // Determining startDate based on dateFilter.
        switch (dateFilter.ToLower())
        {
            case "today":
                startDate = today;
                break;
            case "week":
                int daysToSubtract = (int)today.DayOfWeek;
                startDate = today.AddDays(-daysToSubtract);
                break;
            case "month":
                startDate = new DateTime(today.Year, today.Month, 1);
                break;
        }

        finalCards.AddRange(cards.Where(c => c.DueDate >= startDate && (categoryFilter.ToLower() == "all")? true : c.Status.Equals(statusFilter, StringComparison.OrdinalIgnoreCase) && (categoryFilter.ToLower() == "all")? true: c.Category.Equals(categoryFilter, StringComparison.OrdinalIgnoreCase)));

        return finalCards;
    }
    #endregion


    #region Retrieve Detailed Plan Card
    public async Task<PlanCardDetailDto?> GetPlanDetailsAsync(int assignmentId)
    {
        var assignment = await _progressRepository.GetAssignmentDetailsAsync(assignmentId);
        if (assignment == null) return null;

        // This part is correct
        bool hasCustomDetails = assignment.AssignmentPlanDetails != null && assignment.AssignmentPlanDetails.Any();


        var planDetails = new PlanCardDetailDto
        {
            PlanName = assignment.AssignedWellnessPlan.PlanName,
            Goal = assignment.AssignedWellnessPlan.Goal,
            ImageUrl = assignment.AssignedWellnessPlan.ImageUrl,
            AssignedByDoctorName = $"Dr. {assignment.AssigningDoctor.FirstName} {assignment.AssigningDoctor.LastName}",
            Frequency = $"{assignment.FrequencyCount} times per {assignment.FrequencyUnit}",

            // If hasCustomDetails is true, get data from the AssignmentPlanDetails Table otherwise get it from WellPlanDetails.
            Description = (hasCustomDetails
                ? assignment.AssignmentPlanDetails!.First(d => d.DetailType == "Description").Content
                : assignment.AssignedWellnessPlan.WellnessPlanDetails!.First(d => d.DetailType == "Description").Content),

            Instructions = (hasCustomDetails
                ? assignment.AssignmentPlanDetails!.Where(d => d.DetailType == "Instruction").Select(d => d.Content).ToList()
                : assignment.AssignedWellnessPlan.WellnessPlanDetails!.Where(d => d.DetailType == "Instruction").Select(d => d.Content).ToList()),

            Benefits = (hasCustomDetails
                ? assignment.AssignmentPlanDetails!.Where(d => d.DetailType == "Benefit").Select(d => d.Content).ToList()
                : assignment.AssignedWellnessPlan.WellnessPlanDetails!.Where(d => d.DetailType == "Benefit").Select(d => d.Content).ToList()),

            SafetyPrecautions = (hasCustomDetails
                ? assignment.AssignmentPlanDetails!.Where(d => d.DetailType == "Safety").Select(d => d.Content).ToList()
                : assignment.AssignedWellnessPlan.WellnessPlanDetails!.Where(d => d.DetailType == "Safety").Select(d => d.Content).ToList())
        };

        return planDetails;
    }
    #endregion


    #region Updating Task Status
    public async Task<bool> UpdateTaskStatusAsync(int taskLogId, UpdateTaskStatusDto updateDto)
    {
        var taskLog = await _progressRepository.GetTaskLogByIdAsync(taskLogId);
        if (taskLog == null) return false;

        if (updateDto.IsComplete)
        {
            taskLog.Status = "Completed";
            taskLog.CompletedAt = DateTime.UtcNow;
            taskLog.PatientNotes = updateDto.PatientNotes;
        }
        else // Marking as incomplete
        {
            taskLog.Status = "Pending";
            taskLog.CompletedAt = null;
            taskLog.PatientNotes = null;
        }

        return await _progressRepository.SaveChangesAsync();
    }
    #endregion


    #region Retrieving Dashboard Data
    public async Task<DashboardDto> GetDashboardDataAsync(int patientId, int? year = null)
    {
        var today = DateTime.Today;
        var startOfWeek = today.AddDays(-(int)today.DayOfWeek);
        var startOfYear = new DateTime(today.Year, 1, 1);

        var allLogsForYear = await _progressRepository.GetTaskLogsForPeriodAsync(patientId, startOfYear, today);

        // Weekly Wellness Score
        var currentWeekLogs = allLogsForYear.Where(l => l.DueDate >= today.AddDays(-(int)today.DayOfWeek)).ToList();
        var currentWeekCompleted = currentWeekLogs.Count(l => l.Status == "Completed");
        double wellnessScore = currentWeekLogs.Any() ? (currentWeekCompleted / currentWeekLogs.Count) * 100 : 0;

        // Streaks - Streaks are checked based on an year.
        var logsByDate = allLogsForYear
            .Where(l => l.DueDate <= today) 
            .GroupBy(l => l.DueDate.Date)
            .ToDictionary(g => g.Key, g => g.All(t => t.Status == "Completed"));

        int currentStreak = 0;
        int bestStreak = 0;

        // Check if today's tasks are all complete to determine the actual current streak including today.
        var todayLogs = allLogsForYear.Where(l => l.DueDate == today).ToList();
        if (todayLogs.Any() && todayLogs.All(l => l.Status == "Completed"))
        {
            currentStreak = 1;
        }

        // Loop backwards from yesterday to find only the current streak
        for (var date = today.AddDays(-1); date >= startOfYear; date = date.AddDays(-1))
        {
            if (logsByDate.TryGetValue(date, out bool allCompleted) && allCompleted)
            {
                currentStreak++;
            }
            else
            {
                // The streak is broken, so we stop counting for the current streak
                break;
            }
        }

        // Here we will find all time best streak.
        int longestStreakFound = 0;
        int tempStreak = 0;
        for (var date = today; date >= startOfYear; date = date.AddDays(-1))
        {
            if (logsByDate.TryGetValue(date, out bool allCompleted) && allCompleted)
            {
                tempStreak++;
            }
            else
            {
                if (tempStreak > longestStreakFound)
                {
                    longestStreakFound = tempStreak;
                }
                tempStreak = 0;
            }
        }

        if (tempStreak > longestStreakFound)
        {
            longestStreakFound = tempStreak;
        }
        bestStreak = longestStreakFound;

        // Health Tip
        var tips = new List<string> { "A 2% drop in hydration can impact memory and focus. Keep a water bottle handy!", "Take a 5-minute stretching break for every hour you sit.", "Eating a handful of nuts can be a great source of healthy fats and energy." };
        var healthTip = tips[new Random().Next(tips.Count)];

        // Tasks Today & This Week
        //todayLogs = allLogsForYear.Where(l => l.DueDate == today).ToList();
        var weekLogs = allLogsForYear.Where(l => l.DueDate >= startOfWeek && l.DueDate <= today).ToList();

        return new DashboardDto
        {
            WellnessScore = Math.Round(wellnessScore, 1),
            CurrentStreak = currentStreak,
            BestStreak = bestStreak,
            HealthTipOfTheDay = healthTip,
            TasksTodayCompleted = todayLogs.Count(l => l.Status == "Completed"),
            TasksTodayTotal = todayLogs.Count,
            TasksThisWeekCompleted = weekLogs.Count(l => l.Status == "Completed"),
            TasksThisWeekTotal = weekLogs.Count,
            ActivityCalendar = await GetActivityCalendarAsync(patientId, year ?? (int)today.Year)
        };
    }
    #endregion


    #region Retrieving Activity Calendar Data
    public async Task<List<ActivityCalendarDay>> GetActivityCalendarAsync(int patientId, int year)
    {
        var startOfYear = new DateTime(year, 1, 1);
        var endOfYear = new DateTime(year, 12, 31);

        // Make a targeted database call ONLY for the requested year's data.
        var logsForYear = await _progressRepository.GetTaskLogsForPeriodAsync(patientId, startOfYear, endOfYear);

        // This logic is now isolated to its own method.
        var calendarData = logsForYear
            .GroupBy(l => l.DueDate.Date)
            .Select(g => new ActivityCalendarDay
            {
                Date = g.Key,
                CompletionLevel = g.Count(t => t.Status == "Completed")
            }).ToList();

        return calendarData;
    }
    #endregion
}