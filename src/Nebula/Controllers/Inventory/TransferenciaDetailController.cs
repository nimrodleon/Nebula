using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Database.Helpers;
using Nebula.Modules.Inventory.Models;
using Nebula.Modules.Inventory.Transferencias;

namespace Nebula.Controllers.Inventory;

[Authorize(Roles = AuthRoles.User)]
[Route("api/[controller]")]
[ApiController]
public class TransferenciaDetailController : ControllerBase
{
    private readonly TransferenciaDetailService _transferenciaDetailService;

    public TransferenciaDetailController(TransferenciaDetailService transferenciaDetailService)
    {
        _transferenciaDetailService = transferenciaDetailService;
    }

    [HttpGet("Index/{id}")]
    public async Task<IActionResult> Index(string id)
    {
        var responseData = await _transferenciaDetailService.GetListAsync(id);
        return Ok(responseData);
    }

    [HttpGet("Show/{id}")]
    public async Task<IActionResult> Show(string id)
    {
        var transferenciaDetail = await _transferenciaDetailService.GetByIdAsync(id);
        return Ok(transferenciaDetail);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] TransferenciaDetail model)
    {
        var transferenciaDetail = await _transferenciaDetailService.CreateAsync(model);
        return Ok(transferenciaDetail);
    }

    [HttpPut("Update/{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] TransferenciaDetail model)
    {
        var transferenciaDetail = await _transferenciaDetailService.GetByIdAsync(id);
        model.Id = transferenciaDetail.Id;
        var responseData = await _transferenciaDetailService.UpdateAsync(id, model);
        return Ok(responseData);
    }

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var transferenciaDetail = await _transferenciaDetailService.GetByIdAsync(id);
        await _transferenciaDetailService.RemoveAsync(transferenciaDetail.Id);
        return Ok(transferenciaDetail);
    }

    [HttpGet("CountDocuments/{id}")]
    public async Task<IActionResult> CountDocuments(string id)
    {
        var countDocuments = await _transferenciaDetailService.CountDocumentsAsync(id);
        return Ok(countDocuments);
    }
}
