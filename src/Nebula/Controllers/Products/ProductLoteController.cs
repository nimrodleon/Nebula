using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Auth;
using Nebula.Modules.Auth.Helpers;
using Nebula.Modules.Products;
using Nebula.Modules.Products.Models;

namespace Nebula.Controllers.Products;

[Route("api/[controller]")]
[ApiController]
public class ProductLoteController : ControllerBase
{
    private readonly IProductLoteService _productLoteService;

    public ProductLoteController(IProductLoteService productLoteService)
    {
        _productLoteService = productLoteService;
    }

    [HttpGet("Index/{id}"), UserAuthorize(Permission.ProductRead)]
    public async Task<IActionResult> Index(string id, [FromQuery] string? expirationDate)
    {
        if (string.IsNullOrEmpty(expirationDate))
            expirationDate = DateTime.Now.ToString("yyyy-MM-dd");
        var responseData = await _productLoteService.GetLotesByExpirationDate(id, expirationDate);
        return Ok(responseData);
    }

    [HttpGet("LotesByProduct/{id}"), UserAuthorize(Permission.ProductRead)]
    public async Task<IActionResult> LotesByProduct(string id)
    {
        var responseData = await _productLoteService.GetLotesByProductId(id);
        return Ok(responseData);
    }

    [HttpPost("Create"), UserAuthorize(Permission.ProductCreate)]
    public async Task<IActionResult> Create([FromBody] ProductLote model)
    {
        model.LotNumber = model.LotNumber.ToUpper();
        await _productLoteService.CreateAsync(model);
        await _productLoteService.UpdateProductHasLote(model.ProductId);
        return Ok(model);
    }

    [HttpPut("Update/{id}"), UserAuthorize(Permission.ProductEdit)]
    public async Task<IActionResult> Update(string id, [FromBody] ProductLote model)
    {
        var lote = await _productLoteService.GetByIdAsync(id);
        model.Id = lote.Id;
        model.LotNumber = model.LotNumber.ToUpper();
        model = await _productLoteService.UpdateAsync(id, model);
        return Ok(model);
    }

    [HttpDelete("Delete/{id}"), UserAuthorize(Permission.ProductDelete)]
    public async Task<IActionResult> Delete(string id)
    {
        var lote = await _productLoteService.GetByIdAsync(id);
        await _productLoteService.RemoveAsync(id);
        await _productLoteService.UpdateProductHasLote(lote.ProductId);
        return Ok(lote);
    }
}
