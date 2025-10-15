using System.Collections.Generic;
using System.Threading.Tasks;
using Patient_Monitoring.DTOs;
using Patient_Monitoring.DTOs.WellnessPlan;

namespace Patient_Monitoring.Services.Interface
{
    public interface IWellnessPlanService
    {
        // Use Template Flow
        Task<IEnumerable<TemplateCardDto>> GetAvailableTemplateCards();
        Task<TemplateDetailsDto> GetTemplateDetailsForFormAsync(string planId);

        // Assignment Flow (Handles both Use Template and Create from Scratch)
        Task<string> AssignNewPlanAsync(AssignPlanRequestDto request);
    }
}

