public class AssignedPlanCardDto
{
    public int AssignmentId { get; set; }
    public string ImageUrl { get; set; }
    public string PlanName { get; set; }
    public string Category { get; set; }
    public string Status { get; set; } // "Completed", "Pending", "Missed"
    public DateTime DueDate { get; set; }
    public int TaskLogId { get; set; } // Needed to mark the task as complete
}