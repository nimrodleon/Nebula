using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Database.Helpers;
using Nebula.Database.Models.Inventory;
using Nebula.Database.Services.Inventory;
using Nebula.Database.ViewModels.Common;

namespace Nebula.Controllers.Inventory;

[Authorize(Roles = AuthRoles.User)]
[Route("api/[controller]")]
[ApiController]
public class MaterialController : ControllerBase
{
    private readonly MaterialService _materialService;

    public MaterialController(MaterialService materialService)
    {
        _materialService = materialService;
    }

    [HttpGet("Index")]
    public async Task<IActionResult> Index([FromQuery] DateQuery model)
    {
        var responseData = await _materialService.GetListAsync(model);
        return Ok(responseData);
    }

    [HttpGet("Show/{id}")]
    public async Task<IActionResult> Show(string id)
    {
        var location = await _materialService.GetAsync(id);
        return Ok(location);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] Material model)
    {
        var location = await _materialService.CreateAsync(model);
        return Ok(location);
    }

    [HttpPut("Update/{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] Material model)
    {
        var material = await _materialService.GetAsync(id);
        model.Id = material.Id;
        var responseData = await _materialService.UpdateAsync(id, model);
        return Ok(responseData);
    }

    [HttpDelete("Delete/{id}"), Authorize(Roles = AuthRoles.Admin)]
    public async Task<IActionResult> Delete(string id)
    {
        var material = await _materialService.GetAsync(id);
        await _materialService.RemoveAsync(material.Id);
        return Ok(material);
    }

}
