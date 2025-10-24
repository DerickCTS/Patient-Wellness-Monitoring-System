namespace Patient_Monitoring.DTOs.WellnessPlan
{
    public class TemplateCardDto
    {
        public int PlanId { get; set; }
        public string PlanName { get; set; } = null!;
        public string Goal { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
    }
}
