using Banking.Domain.Enums;
using Banking.Domain.Models;
using Banking.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Banking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FraudDetectionController : ControllerBase
    {
        private readonly IFraudDetectionService _fraudDetectionService;

        public FraudDetectionController(IFraudDetectionService fraudDetectionService)
        {
            _fraudDetectionService = fraudDetectionService;
        }

        [HttpGet("pending-alerts")]
        public ActionResult<IEnumerable<FraudAlert>> GetPendingAlerts()
        {
            return Ok(_fraudDetectionService.GetPendingAlerts());
        }

        [HttpPost("resolve-alert/{alertId}")]
        public ActionResult ResolveAlert(int alertId, [FromBody] FraudAlertResolutionRequest request)
        {
            _fraudDetectionService.ResolveAlert(alertId, request.ResolvedBy, request.Status);
            return Ok();
        }
    }
}
