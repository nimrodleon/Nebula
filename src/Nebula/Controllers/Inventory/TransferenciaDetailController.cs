using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Auth;
using Nebula.Modules.Auth.Helpers;
using Nebula.Modules.Inventory.Models;
using Nebula.Modules.Inventory.Transferencias;

namespace Nebula.Controllers.Inventory;

[Authorize]
[CustomerAuthorize(UserRole = UserRoleHelper.User)]
[Route("api/inventory/{companyId}/[controller]")]
[ApiController]
public class TransferenciaDetailController : ControllerBase
{
    private readonly ITransferenciaDetailService _transferenciaDetailService;

    public TransferenciaDetailController(ITransferenciaDetailService transferenciaDetailService)
    {
        _transferenciaDetailService = transferenciaDetailService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Index(string companyId, string id)
    {
        var responseData = await _transferenciaDetailService.GetListAsync(companyId, id);
        return Ok(responseData);
    }

    [HttpGet("Show/{id}")]
    public async Task<IActionResult> Show(string companyId, string id)
    {
        var transferenciaDetail = await _transferenciaDetailService.GetByIdAsync(companyId, id);
        return Ok(transferenciaDetail);
    }

    [HttpPost]
    public async Task<IActionResult> Create(string companyId, [FromBody] TransferenciaDetail model)
    {
        model.CompanyId = companyId.Trim();
        var transferenciaDetail = await _transferenciaDetailService.CreateAsync(model);
        return Ok(transferenciaDetail);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string companyId, string id, [FromBody] TransferenciaDetail model)
    {
        var transferenciaDetail = await _transferenciaDetailService.GetByIdAsync(companyId, id);
        model.Id = transferenciaDetail.Id;
        model.CompanyId = companyId.Trim();
        var responseData = await _transferenciaDetailService.UpdateAsync(id, model);
        return Ok(responseData);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string companyId, string id)
    {
        var transferenciaDetail = await _transferenciaDetailService.GetByIdAsync(companyId, id);
        await _transferenciaDetailService.RemoveAsync(companyId, transferenciaDetail.Id);
        return Ok(transferenciaDetail);
    }

    [HttpGet("CountDocuments/{id}")]
    public async Task<IActionResult> CountDocuments(string companyId, string id)
    {
        var countDocuments = await _transferenciaDetailService.CountDocumentsAsync(companyId, id);
        return Ok(countDocuments);
    }
}
