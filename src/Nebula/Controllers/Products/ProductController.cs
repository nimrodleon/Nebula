using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Products.Models;
using Nebula.Modules.Products;
using Nebula.Modules.Products.Dto;
using Microsoft.AspNetCore.Authorization;
using Nebula.Modules.Auth.Helpers;
using Nebula.Modules.Auth;

namespace Nebula.Controllers.Products;

[Authorize]
[CustomerAuthorize(UserRole = CompanyRoles.User)]
[Route("api/products/{companyId}/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string companyId, [FromQuery] string query = "")
    {
        string[] fieldNames = new string[] { "Barcode", "Description" };
        var products = await _productService.GetFilteredAsync(companyId, fieldNames, query);
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Show(string companyId, string id)
    {
        var product = await _productService.GetByIdAsync(companyId, id);
        return Ok(product);
    }

    [HttpGet("Select2")]
    public async Task<IActionResult> Select2(string companyId, [FromQuery] string term = "")
    {
        string[] fieldNames = new string[] { "Barcode", "Description" };
        var products = await _productService.GetFilteredAsync(companyId, fieldNames, term, 10);
        var data = new List<ProductSelect>();
        products.ForEach(item =>
        {
            var itemSelect2 = new ProductSelect
            {
                Id = item.Id,
                CompanyId = item.CompanyId,
                Description = item.Description,
                Category = item.Category,
                Barcode = item.Barcode,
                IgvSunat = item.IgvSunat,
                PrecioVentaUnitario = item.PrecioVentaUnitario,
                Type = item.Type,
                UndMedida = item.UndMedida,
                Lote = item.Lote,
                FecVencimiento = item.FecVencimiento,
                Text = $"{item.Description} | {Convert.ToDecimal(item.PrecioVentaUnitario):N2}"
            };
            data.Add(itemSelect2);
        });
        return Ok(new { Results = data });
    }

    [HttpPost]
    public async Task<IActionResult> Create(string companyId, [FromBody] Product model)
    {
        model.CompanyId = companyId.Trim();
        model.Description = model.Description.ToUpper();
        model = await _productService.CreateAsync(model);
        return Ok(model);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string companyId, string id, [FromBody] Product model)
    {
        var product = await _productService.GetByIdAsync(companyId, id);
        if (product == null) return BadRequest();
        model.Description = model.Description.ToUpper();
        product = await _productService.UpdateAsync(product.Id, model);
        return Ok(product);
    }

    [HttpDelete("{id}"), CustomerAuthorize(UserRole = CompanyRoles.Admin)]
    public async Task<IActionResult> Delete(string companyId, string id)
    {
        var product = await _productService.GetByIdAsync(companyId, id);
        await _productService.RemoveAsync(companyId, product.Id);
        return Ok(product);
    }

}
