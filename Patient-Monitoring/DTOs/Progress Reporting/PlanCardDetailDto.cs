public class PlanCardDetailDto
{
    public string ImageUrl { get; set; }
    public string PlanName { get; set; }
    public string Goal { get; set; }
    public string AssignedByDoctorName { get; set; }
    public string Frequency { get; set; }
    public string Description { get; set; }
    public List<string> Instructions { get; set; }
    public List<string> Benefits { get; set; }
    public List<string> SafetyPrecautions { get; set; }
}