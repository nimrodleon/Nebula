using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Common.Dto;
using Nebula.Modules.Auth.Helpers;
using Nebula.Modules.Auth;
using Nebula.Modules.Taller.Models;
using Nebula.Modules.Taller.Services;
using Nebula.Common.Helpers;

namespace Nebula.Controllers.Taller;

[Authorize]
[CustomerAuthorize(UserRole = UserRoleHelper.User)]
[Route("api/taller/{companyId}/[controller]")]
[ApiController]
public class RepairOrderController(ITallerRepairOrderService repairOrderService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Index(string companyId, [FromQuery] string query = "", [FromQuery] int page = 1)
    {
        int pageSize = 12;
        var reparaciones = await repairOrderService.GetRepairOrders(companyId, query, page, pageSize);
        var totalReparaciones = await repairOrderService.GetTotalRepairOrders(companyId, query);
        var totalPages = (int)Math.Ceiling((double)totalReparaciones / pageSize);

        var paginationInfo = new PaginationInfo
        {
            CurrentPage = page,
            TotalPages = totalPages
        };

        paginationInfo.GeneratePageLinks();

        var result = new PaginationResult<TallerRepairOrder>
        {
            Pagination = paginationInfo,
            Data = reparaciones
        };

        return Ok(result);
    }

    [HttpGet("GetMonthlyReport")]
    public async Task<IActionResult> GetMonthlyReport(string companyId, [FromQuery] DateQuery dto, [FromQuery] int page = 1)
    {
        int pageSize = 12;
        var reparaciones = await repairOrderService.GetRepairOrdersMonthly(companyId, dto, page, pageSize);
        var totalReparaciones = await repairOrderService.GetTotalRepairOrdersMonthly(companyId, dto);
        var totalPages = (int)Math.Ceiling((double)totalReparaciones / pageSize);

        var paginationInfo = new PaginationInfo
        {
            CurrentPage = page,
            TotalPages = totalPages
        };

        paginationInfo.GeneratePageLinks();

        var result = new PaginationResult<TallerRepairOrder>
        {
            Pagination = paginationInfo,
            Data = reparaciones
        };

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Show(string companyId, string id)
    {
        var repairOrder = await repairOrderService.GetByIdAsync(companyId, id);
        return Ok(repairOrder);
    }

    [HttpGet("GetTicket/{id}")]
    public async Task<IActionResult> GetTicket(string companyId, string id)
    {
        var ticket = await repairOrderService.GetTicket(companyId, id);
        return Ok(ticket);
    }

    [HttpPost]
    public async Task<IActionResult> Create(string companyId, [FromBody] TallerRepairOrder model)
    {
        await repairOrderService.CreateRepairOrderAsync(companyId, model);
        return Ok(model);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string companyId, string id, [FromBody] TallerRepairOrder model)
    {
        var repairOrder = await repairOrderService.GetByIdAsync(companyId, id);
        model.Id = repairOrder.Id;
        model.CompanyId = companyId.Trim();
        model.Serie = repairOrder.Serie;
        model.Number = repairOrder.Number;
        model.CreatedAt = repairOrder.CreatedAt;
        model.UpdatedAt = repairOrder.UpdatedAt;
        model.Year = repairOrder.Year;
        model.Month = repairOrder.Month;
        if (repairOrder.Status != model.Status)
            model.UpdatedAt = DateTime.Now.ToString("yyyy-MM-dd");
        repairOrder = await repairOrderService.ReplaceOneAsync(repairOrder.Id, model);
        return Ok(repairOrder);
    }

    [HttpDelete("{id}"), CustomerAuthorize(UserRole = UserRoleHelper.Admin)]
    public async Task<IActionResult> Delete(string companyId, string id)
    {
        var repairOrder = await repairOrderService.GetByIdAsync(companyId, id);
        await repairOrderService.DeleteOneAsync(companyId, repairOrder.Id);
        return Ok(repairOrder);
    }
}
