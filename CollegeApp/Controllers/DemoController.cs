using CollegeApp.MyLogging;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CollegeApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors(PolicyName = "AllowOnlyGoogle")]
    public class DemoController : ControllerBase
    {
        private readonly ILogger<DemoController> _logger;
        public DemoController(ILogger<DemoController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        public ActionResult Index()
        {
            _logger.LogTrace("Log message from trace");
            _logger.LogDebug("Log message from debug");
            _logger.LogInformation("Log message from information");
            _logger.LogWarning("Log message from warning");
            _logger.LogError("Log message from error");
            _logger.LogCritical("Log message from critical");
            return Ok();
        }
    }
}
