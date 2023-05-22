using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Database.Helpers;
using Nebula.Modules.Inventory.Materiales;
using Nebula.Modules.Inventory.Models;

namespace Nebula.Controllers.Inventory;

[Authorize(Roles = AuthRoles.User)]
[Route("api/[controller]")]
[ApiController]
public class MaterialDetailController : ControllerBase
{
    private readonly MaterialDetailService _materialDetailService;

    public MaterialDetailController(MaterialDetailService materialDetailService)
    {
        _materialDetailService = materialDetailService;
    }

    [HttpGet("Index/{id}")]
    public async Task<IActionResult> Index(string id)
    {
        var responseData = await _materialDetailService.GetListAsync(id);
        return Ok(responseData);
    }

    [HttpGet("Show/{id}")]
    public async Task<IActionResult> Show(string id)
    {
        var materialDetail = await _materialDetailService.GetByIdAsync(id);
        return Ok(materialDetail);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] MaterialDetail model)
    {
        var materialDetail = await _materialDetailService.CreateAsync(model);
        return Ok(materialDetail);
    }

    [HttpPut("Update/{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] MaterialDetail model)
    {
        var materialDetail = await _materialDetailService.GetByIdAsync(id);
        model.Id = materialDetail.Id;
        var responseData = await _materialDetailService.UpdateAsync(id, model);
        return Ok(responseData);
    }

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var materialDetail = await _materialDetailService.GetByIdAsync(id);
        await _materialDetailService.RemoveAsync(materialDetail.Id);
        return Ok(materialDetail);
    }

    [HttpGet("CountDocuments/{id}")]
    public async Task<IActionResult> CountDocuments(string id)
    {
        var countDocuments = await _materialDetailService.CountDocumentsAsync(id);
        return Ok(countDocuments);
    }
}
