using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Configurations.Subscriptions;

namespace Nebula.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TestsController : ControllerBase
{
    [HttpGet("GetProcessorId")]
    public IActionResult GetProcessorId()
    {
        var processorId = new MachineUUID().GetProcessorId();
        return Ok(processorId);
    }
}
