using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Auth;
using Nebula.Modules.Auth.Helpers;
using Nebula.Modules.Inventory.Models;
using Nebula.Modules.Inventory.Transferencias;

namespace Nebula.Controllers.Inventory;

[Authorize]
[CustomerAuthorize(UserRole = UserRoleHelper.User)]
[Route("api/inventory/[controller]")]
[ApiController]
public class TransferenciaDetailController(
    IUserAuthenticationService userAuthenticationService,
    ITransferenciaDetailService transferenciaDetailService) : ControllerBase
{
    private readonly string _companyId = userAuthenticationService.GetDefaultCompanyId();

    [HttpGet("{id}")]
    public async Task<IActionResult> Index(string id)
    {
        var responseData = await transferenciaDetailService.GetListAsync(_companyId, id);
        return Ok(responseData);
    }

    [HttpGet("Show/{id}")]
    public async Task<IActionResult> Show(string id)
    {
        var transferenciaDetail = await transferenciaDetailService.GetByIdAsync(_companyId, id);
        return Ok(transferenciaDetail);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TransferenciaDetail model)
    {
        model.CompanyId = _companyId.Trim();
        var transferenciaDetail = await transferenciaDetailService.InsertOneAsync(model);
        return Ok(transferenciaDetail);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] TransferenciaDetail model)
    {
        var transferenciaDetail = await transferenciaDetailService.GetByIdAsync(_companyId, id);
        model.Id = transferenciaDetail.Id;
        model.CompanyId = _companyId.Trim();
        var responseData = await transferenciaDetailService.ReplaceOneAsync(id, model);
        return Ok(responseData);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var transferenciaDetail = await transferenciaDetailService.GetByIdAsync(_companyId, id);
        await transferenciaDetailService.DeleteOneAsync(_companyId, transferenciaDetail.Id);
        return Ok(transferenciaDetail);
    }

    [HttpGet("CountDocuments/{id}")]
    public async Task<IActionResult> CountDocuments(string id)
    {
        var countDocuments = await transferenciaDetailService.CountDocumentsAsync(_companyId, id);
        return Ok(countDocuments);
    }
}
