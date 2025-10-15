namespace Patient_Monitoring.DTOs.WellnessPlan
{
    public class TemplateCardDto
    {
        public string PlanId { get; set; } = null!;
        public string PlanName { get; set; } = null!;
        public string Goal { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
    }
}
