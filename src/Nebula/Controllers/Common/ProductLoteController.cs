using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Database.Helpers;
using Nebula.Database.Models.Common;
using Nebula.Database.Services.Common;

namespace Nebula.Controllers.Common;

[Route("api/[controller]")]
[ApiController]
public class ProductLoteController : ControllerBase
{
    private readonly ProductLoteService _productLoteService;

    public ProductLoteController(ProductLoteService productLoteService)
    {
        _productLoteService = productLoteService;
    }

    [HttpGet("Index/{id}")]
    public async Task<IActionResult> Index(string id, [FromQuery] string? expirationDate)
    {
        if (string.IsNullOrEmpty(expirationDate))
            expirationDate = DateTime.Now.ToString("yyyy-MM-dd");
        var responseData = await _productLoteService.GetLotesByExpirationDate(id, expirationDate);
        return Ok(responseData);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] ProductLote model)
    {
        model.LotNumber = model.LotNumber.ToUpper();
        await _productLoteService.CreateAsync(model);
        return Ok(model);
    }

    [HttpPut("Update/{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] ProductLote model)
    {
        var lote = await _productLoteService.GetByIdAsync(id);
        model.Id = lote.Id;
        model.LotNumber = model.LotNumber.ToUpper();
        model = await _productLoteService.UpdateAsync(id, model);
        return Ok(model);
    }

    [HttpDelete("Delete/{id}"), Authorize(Roles = AuthRoles.Admin)]
    public async Task<IActionResult> Delete(string id)
    {
        var lote = await _productLoteService.GetByIdAsync(id);
        await _productLoteService.RemoveAsync(id);
        return Ok(lote);
    }
}