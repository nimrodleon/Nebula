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
    [HttpGet("GetUUID")]
    public IActionResult GetUUID()
    {
        var uuid = new MachineUUID().GetUUID();
        return Ok(uuid);
    }

    [HttpGet("GetProcessorId")]
    public IActionResult GetProcessorId()
    {
        var processorId = new MachineUUID().GetProcessorId();
        return Ok(processorId);
    }

    [HttpGet("GetHardDriveModel")]
    public IActionResult GetHardDriveModel()
    {
        var model = new MachineUUID().GetHardDriveModel();
        return Ok(model);
    }

    [HttpGet("GetHardDriveSerialNumber")]
    public IActionResult GetHardDriveSerialNumber()
    {
        var serialNumber = new MachineUUID().GetHardDriveSerialNumber();
        return Ok(serialNumber);
    }

    [HttpGet("GetMotherboardProductInfo")]
    public IActionResult GetMotherboardProductInfo()
    {
        var productInfo = new MachineUUID().GetMotherboardProductInfo();
        return Ok(productInfo);
    }
}
