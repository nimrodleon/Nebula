using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Database.Dto.Common;
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
        var repairOrders = await _repairOrderService.GetRepairOrders(query);
        return Ok(repairOrders);
    }

    [HttpGet("GetMonthlyReport")]
    public async Task<IActionResult> GetMonthlyReport([FromQuery] DateQuery dto)
    {
        var repairOrders = await _repairOrderService.GetRepairOrdersMonthly(dto);
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
        model.CreatedAt = repairOrder.CreatedAt;
        model.UpdatedAt = repairOrder.UpdatedAt;
        model.Year = repairOrder.Year;
        model.Month = repairOrder.Month;
        if (repairOrder.Status != model.Status)
            model.UpdatedAt = DateTime.Now.ToString("yyyy-MM-dd");
        repairOrder = await _repairOrderService.UpdateAsync(repairOrder.Id, model);
        return Ok(repairOrder);
    }

    [HttpDelete("Delete/{id}"), Authorize(Roles = AuthRoles.Admin)]
    public async Task<IActionResult> Delete(string id)
    {
        var repairOrder = await _repairOrderService.GetByIdAsync(id);
        await _repairOrderService.RemoveAsync(repairOrder.Id);
        return Ok(repairOrder);
    }
}
