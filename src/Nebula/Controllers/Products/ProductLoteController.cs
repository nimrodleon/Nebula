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
public class ProductLoteController : ControllerBase
{
    private readonly IProductLoteService _productLoteService;

    public ProductLoteController(IProductLoteService productLoteService)
    {
        _productLoteService = productLoteService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Index(string companyId, string id, [FromQuery] string? expirationDate)
    {
        if (string.IsNullOrEmpty(expirationDate))
            expirationDate = DateTime.Now.ToString("yyyy-MM-dd");
        var responseData = await _productLoteService.GetLotesByExpirationDate(companyId, id, expirationDate);
        return Ok(responseData);
    }

    [HttpGet("LotesByProduct/{id}")]
    public async Task<IActionResult> LotesByProduct(string companyId, string id)
    {
        var responseData = await _productLoteService.GetLotesByProductId(companyId, id);
        return Ok(responseData);
    }

    [HttpPost]
    public async Task<IActionResult> Create(string companyId, [FromBody] ProductLote model)
    {
        model.LotNumber = model.LotNumber.ToUpper();
        await _productLoteService.CreateAsync(model);
        await _productLoteService.UpdateProductHasLote(companyId, model.ProductId);
        return Ok(model);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string companyId, string id, [FromBody] ProductLote model)
    {
        var lote = await _productLoteService.GetByIdAsync(companyId, id);
        model.Id = lote.Id;
        model.CompanyId = companyId.Trim();
        model.LotNumber = model.LotNumber.ToUpper();
        model = await _productLoteService.UpdateAsync(id, model);
        return Ok(model);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string companyId, string id)
    {
        var lote = await _productLoteService.GetByIdAsync(companyId, id);
        await _productLoteService.RemoveAsync(companyId, id);
        await _productLoteService.UpdateProductHasLote(companyId, lote.ProductId);
        return Ok(lote);
    }
}
