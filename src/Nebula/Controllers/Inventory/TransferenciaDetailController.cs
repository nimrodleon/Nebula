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
public class TransferenciaDetailController(ITransferenciaDetailService transferenciaDetailService) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> Index(string companyId, string id)
    {
        var responseData = await transferenciaDetailService.GetListAsync(companyId, id);
        return Ok(responseData);
    }

    [HttpGet("Show/{id}")]
    public async Task<IActionResult> Show(string companyId, string id)
    {
        var transferenciaDetail = await transferenciaDetailService.GetByIdAsync(companyId, id);
        return Ok(transferenciaDetail);
    }

    [HttpPost]
    public async Task<IActionResult> Create(string companyId, [FromBody] TransferenciaDetail model)
    {
        model.CompanyId = companyId.Trim();
        var transferenciaDetail = await transferenciaDetailService.InsertOneAsync(model);
        return Ok(transferenciaDetail);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string companyId, string id, [FromBody] TransferenciaDetail model)
    {
        var transferenciaDetail = await transferenciaDetailService.GetByIdAsync(companyId, id);
        model.Id = transferenciaDetail.Id;
        model.CompanyId = companyId.Trim();
        var responseData = await transferenciaDetailService.ReplaceOneAsync(id, model);
        return Ok(responseData);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string companyId, string id)
    {
        var transferenciaDetail = await transferenciaDetailService.GetByIdAsync(companyId, id);
        await transferenciaDetailService.DeleteOneAsync(companyId, transferenciaDetail.Id);
        return Ok(transferenciaDetail);
    }

    [HttpGet("CountDocuments/{id}")]
    public async Task<IActionResult> CountDocuments(string companyId, string id)
    {
        var countDocuments = await transferenciaDetailService.CountDocumentsAsync(companyId, id);
        return Ok(countDocuments);
    }
}
