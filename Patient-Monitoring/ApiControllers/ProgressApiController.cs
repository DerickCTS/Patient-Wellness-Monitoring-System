//using Microsoft.AspNetCore.Mvc;
//using Patient_Monitoring.Services.Interfaces;

//namespace Patient_Monitoring.ApiControllers
//{
//    [ApiController]
//    [Route("patients/{patientId}/progress")]
//    public class ProgressApiController : ControllerBase
//    {
//        private readonly IProgressService _progressService;

//        public ProgressApiController(IProgressService progressService)
//        {
//            _progressService = progressService;
//        }

//        [HttpGet("plans")]
//        public async Task<IActionResult> GetAssignedPlans(string patientId, [FromQuery] string timeframe = "This Week")
//        {
//            var plans = await _progressService.GetAssignedPlansAsync(patientId, timeframe);
//            return Ok(plans);
//        }

//        [HttpGet("logs/{logId}/details")]
//        public async Task<IActionResult> GetPlanDetails(int patientId, int logId)
//        {
//            // You might add a check here to ensure the log belongs to the patient
//            var details = await _progressService.GetPlanDetailsAsync(logId);
//            if (details == null)
//            {
//                return NotFound();
//            }
//            return Ok(details);
//        }

//        [HttpPost("logs/{logId}/status")]
//        public async Task<IActionResult> UpdateTaskStatus(int patientId, int logId, [FromBody] UpdateTaskStatusRequest request)
//        {
//            // Add check to ensure log belongs to patient
//            var result = await _progressService.UpdateTaskStatusAsync(logId, request.NewStatus);
//            if (!result)
//            {
//                return BadRequest("Failed to update task status.");
//            }
//            return Ok();
//        }
//    }
//}
