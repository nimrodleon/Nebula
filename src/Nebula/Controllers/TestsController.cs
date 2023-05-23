using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Auth.Helpers;
using Nebula.Modules.Configurations.Subscriptions;

namespace Nebula.Controllers;

[Authorize(Roles = AuthRoles.User)]
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
