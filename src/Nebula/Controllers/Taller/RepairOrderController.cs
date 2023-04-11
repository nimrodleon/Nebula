using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Database.Helpers;
using Nebula.Plugins.Taller.Models;
using Nebula.Plugins.Taller.Services;

namespace Nebula.Controllers.Taller;

[Authorize(Roles = AuthRoles.User)]
[Route("api/taller/[controller]")]
[ApiController]
public class RepairOrderController : ControllerBase
{
    private readonly TallerRepairOrderService _repairOrderService;

    public RepairOrderController(TallerRepairOrderService repairOrderService)
    {
        _repairOrderService = repairOrderService;
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] TallerRepairOrder model)
    {
        await _repairOrderService.CreateAsync(model);
        return Ok(model);
    }
}
