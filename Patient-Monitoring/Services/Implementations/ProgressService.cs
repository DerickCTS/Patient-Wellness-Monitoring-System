public class ProgressService : IProgressService
{
    private readonly IProgressRepository _progressRepository;

    public ProgressService(IProgressRepository progressRepository)
    {
        _progressRepository = progressRepository;
    }

    public async Task<List<AssignedPlanCardDto>> GetAssignedPlanCardsAsync(string patientId, string statusFilter, string categoryFilter, string dateFilter)
    {
        var assignments = await _progressRepository.GetActiveAssignmentsWithTasksAsync(patientId);
        var today = DateTime.Today;
        var startOfWeek = today.AddDays(-(int)today.DayOfWeek);
        var endOfWeek = startOfWeek.AddDays(6);

        var cards = new List<AssignedPlanCardDto>();

        foreach (var assignment in assignments)
        {
            // Find the most relevant task log for this assignment
            var relevantTask = assignment.TaskLogs
                .Where(t => t.DueDate >= startOfWeek)
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
        if (!string.IsNullOrEmpty(statusFilter) && statusFilter.ToLower() != "all")
        {
            cards = cards.Where(c => c.Status.Equals(statusFilter, StringComparison.OrdinalIgnoreCase)).ToList();
        }
        if (!string.IsNullOrEmpty(categoryFilter) && categoryFilter.ToLower() != "all")
        {
            cards = cards.Where(c => c.Category.Equals(categoryFilter, StringComparison.OrdinalIgnoreCase)).ToList();
        }
        if (dateFilter.ToLower() == "this week")
        {
            cards = cards.Where(c => c.DueDate >= startOfWeek && c.DueDate <= endOfWeek).ToList();
        }

        return cards;
    }

    public async Task<PlanDetailDto?> GetPlanDetailsAsync(string assignmentId)
    {
        var assignment = await _progressRepository.GetAssignmentDetailsAsync(assignmentId);
        if (assignment == null) return null;

        // This part is correct
        bool hasCustomDetails = assignment.AssignmentPlanDetails != null && assignment.AssignmentPlanDetails.Any();

        // THE FIX: Apply the conditional logic to each property individually to get the final result.
        var planDetails = new PlanDetailDto
        {
            PlanName = assignment.AssignedWellnessPlan.PlanName,
            Goal = assignment.AssignedWellnessPlan.Goal,
            ImageUrl = assignment.AssignedWellnessPlan.ImageUrl,
            AssignedByDoctorName = $"Dr. {assignment.AssigningDoctor.FirstName} {assignment.AssigningDoctor.LastName}",
            Frequency = $"{assignment.FrequencyCount} times per {assignment.FrequencyUnit}",

            // If hasCustomDetails is true, get Description from the first list, otherwise get it from the second.
            Description = (hasCustomDetails
                ? assignment.AssignmentPlanDetails!.FirstOrDefault(d => d.DetailType == "Description")?.Content
                : assignment.AssignedWellnessPlan.WellnessPlanDetails?.FirstOrDefault(d => d.DetailType == "Description")?.Content)
                ?? "No description available.",

            // Do the same for Instructions...
            Instructions = (hasCustomDetails
                ? assignment.AssignmentPlanDetails!.Where(d => d.DetailType == "Instruction").Select(d => d.Content).ToList()
                : assignment.AssignedWellnessPlan.WellnessPlanDetails?.Where(d => d.DetailType == "Instruction").Select(d => d.Content).ToList())
                ?? new List<string>(),

            // ...for Benefits...
            Benefits = (hasCustomDetails
                ? assignment.AssignmentPlanDetails!.Where(d => d.DetailType == "Benefit").Select(d => d.Content).ToList()
                : assignment.AssignedWellnessPlan.WellnessPlanDetails?.Where(d => d.DetailType == "Benefit").Select(d => d.Content).ToList())
                ?? new List<string>(),

            // ...and for Safety.
            SafetyPrecautions = (hasCustomDetails
                ? assignment.AssignmentPlanDetails!.Where(d => d.DetailType == "Safety").Select(d => d.Content).ToList()
                : assignment.AssignedWellnessPlan.WellnessPlanDetails?.Where(d => d.DetailType == "Safety").Select(d => d.Content).ToList())
                ?? new List<string>()
        };

        return planDetails;
    }

    public async Task<bool> UpdateTaskStatusAsync(string taskLogId, UpdateTaskStatusDto updateDto)
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

    public async Task<DashboardDto> GetDashboardDataAsync(string patientId)
    {
        var today = DateTime.Today;
        var startOfWeek = today.AddDays(-(int)today.DayOfWeek);
        var startOfYear = new DateTime(today.Year, 1, 1);

        var allLogsForYear = await _progressRepository.GetTaskLogsForPeriodAsync(patientId, startOfYear, today);

        // Wellness Score
        var last7DaysLogs = allLogsForYear.Where(l => l.DueDate >= today.AddDays(-6)).ToList();
        var completedLast7Days = last7DaysLogs.Count(l => l.Status == "Completed");
        double wellnessScore = last7DaysLogs.Any() ? ((double)completedLast7Days / last7DaysLogs.Count) * 100 : 0;

        // Streaks
        var logsByDate = allLogsForYear
            .Where(l => l.DueDate < today) // Only look at past days for streaks
            .GroupBy(l => l.DueDate.Date)
            .ToDictionary(g => g.Key, g => g.All(t => t.Status == "Completed"));

        int currentStreak = 0;
        int bestStreak = 0;
        int maxStreak = 0;

        for (var date = today.AddDays(-1); date >= startOfYear; date = date.AddDays(-1))
        {
            if (logsByDate.TryGetValue(date, out bool allCompleted) && allCompleted)
            {
                currentStreak++;
            }
            else if (logsByDate.ContainsKey(date)) // Day exists but not all tasks were completed
            {
                if (currentStreak > maxStreak) maxStreak = currentStreak;
                if (bestStreak == 0) bestStreak = currentStreak; // Set best streak on the first break
                currentStreak = 0;
            }
            // If the date is missing, we assume it's a break in the streak
            else if (!logsByDate.ContainsKey(date))
            {
                if (currentStreak > maxStreak) maxStreak = currentStreak;
                currentStreak = 0;
            }
        }
        if (currentStreak > maxStreak) maxStreak = currentStreak;
        bestStreak = maxStreak;

        // Health Tip
        var tips = new List<string> { "A 2% drop in hydration can impact memory and focus. Keep a water bottle handy!", "Take a 5-minute stretching break for every hour you sit.", "Eating a handful of nuts can be a great source of healthy fats and energy." };
        var healthTip = tips[new Random().Next(tips.Count)];

        // Tasks Today & This Week
        var todayLogs = allLogsForYear.Where(l => l.DueDate == today).ToList();
        var weekLogs = allLogsForYear.Where(l => l.DueDate >= startOfWeek && l.DueDate < today.AddDays(1)).ToList();

        // Calendar
        var calendarData = allLogsForYear
            .GroupBy(l => l.DueDate.Date)
            .Select(g => new ActivityCalendarDay
            {
                Date = g.Key,
                CompletionLevel = g.Count(t => t.Status == "Completed")
            }).ToList();

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
            ActivityCalendar = calendarData
        };
    }
}