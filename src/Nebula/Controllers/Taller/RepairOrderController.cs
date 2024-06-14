using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Common.Dto;
using Nebula.Modules.Auth;
using Nebula.Modules.Taller.Models;
using Nebula.Modules.Taller.Services;
using Nebula.Common.Helpers;

namespace Nebula.Controllers.Taller;

[Authorize]
[PersonalAuthorize(UserRole = UserRole.User)]
[Route("api/taller/[controller]")]
[ApiController]
public class RepairOrderController(
    IUserAuthenticationService userAuthenticationService,
    ITallerRepairOrderService repairOrderService) : ControllerBase
{
    private readonly string _companyId = userAuthenticationService.GetDefaultCompanyId();

    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] string query = "", [FromQuery] int page = 1)
    {
        int pageSize = 12;
        var reparaciones = await repairOrderService.GetRepairOrders(_companyId, query, page, pageSize);
        var totalReparaciones = await repairOrderService.GetTotalRepairOrders(_companyId, query);
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
    public async Task<IActionResult> GetMonthlyReport([FromQuery] DateQuery dto, [FromQuery] int page = 1)
    {
        int pageSize = 12;
        var reparaciones = await repairOrderService.GetRepairOrdersMonthly(_companyId, dto, page, pageSize);
        var totalReparaciones = await repairOrderService.GetTotalRepairOrdersMonthly(_companyId, dto);
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
    public async Task<IActionResult> Show(string id)
    {
        var repairOrder = await repairOrderService.GetByIdAsync(_companyId, id);
        return Ok(repairOrder);
    }

    [HttpGet("GetTicket/{id}")]
    public async Task<IActionResult> GetTicket(string id)
    {
        var ticket = await repairOrderService.GetTicket(_companyId, id);
        return Ok(ticket);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TallerRepairOrder model)
    {
        await repairOrderService.CreateRepairOrderAsync(_companyId, model);
        return Ok(model);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] TallerRepairOrder model)
    {
        var repairOrder = await repairOrderService.GetByIdAsync(_companyId, id);
        model.Id = repairOrder.Id;
        model.CompanyId = _companyId.Trim();
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

    [HttpDelete("{id}"), PersonalAuthorize(UserRole = UserRole.Admin)]
    public async Task<IActionResult> Delete(string id)
    {
        var repairOrder = await repairOrderService.GetByIdAsync(_companyId, id);
        await repairOrderService.DeleteOneAsync(_companyId, repairOrder.Id);
        return Ok(repairOrder);
    }
}
