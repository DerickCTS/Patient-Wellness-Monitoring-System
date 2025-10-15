public class AssignPlanRequest
{
    public required string Category { get; set; }
    public string PatientId { get; set; } = null!;
    public string? DoctorId { get; set; }
    public string? PlanId { get; set; } // Optional: null if not using a template

    // These fields are required only if the template is modified or not used
    public string? PlanName { get; set; }
    public string? ImageUrl { get; set; }
    public string? Goal { get; set; }
    public string? Description { get; set; }
    public List<string>? Instruction { get; set; }
    public List<string>? Benefits { get; set; }
    public List<string>? Safety { get; set; }

    public int FrequencyCount { get; set; }

    public string? FrequencyUnit { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }
}