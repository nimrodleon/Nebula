using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Auth;
using Nebula.Modules.Auth.Helpers;
using Nebula.Modules.Products;
using Nebula.Modules.Products.Models;

namespace Nebula.Controllers.Products;

[Route("api/[controller]")]
[ApiController]
public class ProductPricesController : ControllerBase
{
    private readonly IProductPriceService _productPriceService;

    public ProductPricesController(IProductPriceService productPriceService)
    {
        _productPriceService = productPriceService;
    }

    [HttpGet("Index/{productId}")]
    public async Task<IActionResult> Index(string productId)
    {
        var productPrices = await _productPriceService.GetAsync(productId);
        return Ok(productPrices);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] ProductPrices model)
    {
        model.Nombre = model.Nombre.ToUpper();
        await _productPriceService.CreateAsync(model);
        await _productPriceService.UpdateProductHasPrices(model.ProductId);
        return Ok(model);
    }

    [HttpPut("Update/{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] ProductPrices model)
    {
        var price = await _productPriceService.GetByIdAsync(id);
        model.Id = price.Id;
        model.Nombre = model.Nombre.ToUpper();
        model = await _productPriceService.UpdateAsync(model.Id, model);
        return Ok(model);
    }

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var price = await _productPriceService.GetByIdAsync(id);
        await _productPriceService.RemoveAsync(price.Id);
        await _productPriceService.UpdateProductHasPrices(price.ProductId);
        return Ok(price);
    }
}
