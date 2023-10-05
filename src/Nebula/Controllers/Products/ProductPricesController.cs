using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Auth;
using Nebula.Modules.Auth.Helpers;
using Nebula.Modules.Products;
using Nebula.Modules.Products.Models;

namespace Nebula.Controllers.Products;

[Authorize]
[CustomerAuthorize(UserRole = CompanyRoles.User)]
[Route("api/products/{companyId}/[controller]")]
[ApiController]
public class ProductPricesController : ControllerBase
{
    private readonly IProductPriceService _productPriceService;

    public ProductPricesController(IProductPriceService productPriceService)
    {
        _productPriceService = productPriceService;
    }

    [HttpGet("{productId}")]
    public async Task<IActionResult> Index(string companyId, string productId)
    {
        var productPrices = await _productPriceService.GetAsync(companyId, productId);
        return Ok(productPrices);
    }

    [HttpPost]
    public async Task<IActionResult> Create(string companyId, [FromBody] ProductPrices model)
    {
        model.CompanyId = companyId.Trim();
        model.Nombre = model.Nombre.ToUpper();
        await _productPriceService.CreateAsync(model);
        await _productPriceService.UpdateProductHasPrices(companyId, model.ProductId);
        return Ok(model);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string companyId, string id, [FromBody] ProductPrices model)
    {
        var price = await _productPriceService.GetByIdAsync(id);
        model.Id = price.Id;
        model.CompanyId = companyId.Trim();
        model.Nombre = model.Nombre.ToUpper();
        model = await _productPriceService.UpdateAsync(model.Id, model);
        return Ok(model);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string companyId, string id)
    {
        var price = await _productPriceService.GetByIdAsync(companyId, id);
        await _productPriceService.RemoveAsync(companyId, price.Id);
        await _productPriceService.UpdateProductHasPrices(companyId, price.ProductId);
        return Ok(price);
    }
}
