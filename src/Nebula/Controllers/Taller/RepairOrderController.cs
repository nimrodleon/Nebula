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

    [HttpGet("Index")]
    public async Task<IActionResult> Index([FromQuery] string? query)
    {
        var repairOrders = await _repairOrderService.GetAsync("NombreCliente", query);
        return Ok(repairOrders);
    }

    [HttpGet("Show/{id}")]
    public async Task<IActionResult> Show(string id)
    {
        var repairOrder = await _repairOrderService.GetByIdAsync(id);
        return Ok(repairOrder);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] TallerRepairOrder model)
    {
        await _repairOrderService.CreateAsync(model);
        return Ok(model);
    }

    [HttpPut("Update/{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] TallerRepairOrder model)
    {
        var repairOrder = await _repairOrderService.GetByIdAsync(id);
        model.Id = repairOrder.Id;
        repairOrder = await _repairOrderService.UpdateAsync(repairOrder.Id, model);
        return Ok(repairOrder);
    }
}
