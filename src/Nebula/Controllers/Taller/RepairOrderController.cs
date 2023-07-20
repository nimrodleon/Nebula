using Microsoft.AspNetCore.Mvc;
using Nebula.Common.Dto;
using Nebula.Modules.Auth;
using Nebula.Modules.Auth.Helpers;
using Nebula.Modules.Taller.Models;
using Nebula.Modules.Taller.Services;

namespace Nebula.Controllers.Taller;

[Route("api/taller/[controller]")]
[ApiController]
public class RepairOrderController : ControllerBase
{
    private readonly ITallerRepairOrderService _repairOrderService;

    public RepairOrderController(ITallerRepairOrderService repairOrderService)
    {
        _repairOrderService = repairOrderService;
    }

    [HttpGet("Index"), UserAuthorize(Permission.TallerRead)]
    public async Task<IActionResult> Index([FromQuery] string? query)
    {
        var repairOrders = await _repairOrderService.GetRepairOrders(query);
        return Ok(repairOrders);
    }

    [HttpGet("GetMonthlyReport"), UserAuthorize(Permission.TallerRead)]
    public async Task<IActionResult> GetMonthlyReport([FromQuery] DateQuery dto)
    {
        var repairOrders = await _repairOrderService.GetRepairOrdersMonthly(dto);
        return Ok(repairOrders);
    }

    [HttpGet("Show/{id}"), UserAuthorize(Permission.TallerRead)]
    public async Task<IActionResult> Show(string id)
    {
        var repairOrder = await _repairOrderService.GetByIdAsync(id);
        return Ok(repairOrder);
    }

    [HttpGet("GetTicket/{id}"), UserAuthorize(Permission.TallerRead)]
    public async Task<IActionResult> GetTicket(string id)
    {
        var ticket = await _repairOrderService.GetTicket(id);
        return Ok(ticket);
    }

    [HttpPost("Create"), UserAuthorize(Permission.TallerCreate)]
    public async Task<IActionResult> Create([FromBody] TallerRepairOrder model)
    {
        await _repairOrderService.CreateRepairOrderAsync(model);
        return Ok(model);
    }

    [HttpPut("Update/{id}"), UserAuthorize(Permission.TallerEdit)]
    public async Task<IActionResult> Update(string id, [FromBody] TallerRepairOrder model)
    {
        var repairOrder = await _repairOrderService.GetByIdAsync(id);
        model.Id = repairOrder.Id;
        model.Serie = repairOrder.Serie;
        model.Number = repairOrder.Number;
        model.CreatedAt = repairOrder.CreatedAt;
        model.UpdatedAt = repairOrder.UpdatedAt;
        model.Year = repairOrder.Year;
        model.Month = repairOrder.Month;
        if (repairOrder.Status != model.Status)
            model.UpdatedAt = DateTime.Now.ToString("yyyy-MM-dd");
        repairOrder = await _repairOrderService.UpdateAsync(repairOrder.Id, model);
        return Ok(repairOrder);
    }

    [HttpDelete("Delete/{id}"), UserAuthorize(Permission.TallerDelete)]
    public async Task<IActionResult> Delete(string id)
    {
        var repairOrder = await _repairOrderService.GetByIdAsync(id);
        await _repairOrderService.RemoveAsync(repairOrder.Id);
        return Ok(repairOrder);
    }
}
