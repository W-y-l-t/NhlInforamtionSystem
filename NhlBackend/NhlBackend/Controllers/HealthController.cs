using Microsoft.AspNetCore.Mvc;

namespace NhlBackend.Controllers;

[Route("health")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public ActionResult<string> HealthCheck() => Ok("Healthy");
}