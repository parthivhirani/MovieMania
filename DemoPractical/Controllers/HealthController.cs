using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Net;

namespace DemoPractical.Controllers
{
    [Controller]
    [Route("healthCheck")]
    public class HealthController : ControllerBase
    {
        private readonly ILogger<HealthController> _logger;
        private readonly HealthCheckService _healthCheckService;

        public HealthController(ILogger<HealthController> logger, HealthCheckService healthCheckService)
        {
            _logger = logger;
            _healthCheckService = healthCheckService;
        }

        [HttpGet]
        public async Task<IActionResult> GetHealthInfo()
        {
            var report = await _healthCheckService.CheckHealthAsync();
            return report.Status == HealthStatus.Healthy ? Ok(report) : StatusCode((int)HttpStatusCode.ServiceUnavailable, report);
        }
    }
}
