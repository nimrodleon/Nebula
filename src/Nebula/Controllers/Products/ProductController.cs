using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Products.Models;
using Nebula.Modules.Products;
using Nebula.Modules.Products.Dto;
using Microsoft.AspNetCore.Authorization;
using Nebula.Modules.Auth.Helpers;
using Nebula.Modules.Auth;
using Nebula.Modules.Products.Helpers;
using Nebula.Common.Helpers;

namespace Nebula.Controllers.Products;

[Authorize]
[CustomerAuthorize(UserRole = UserRoleHelper.User)]
[Route("api/products/{companyId}/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;

    public ProductController(IProductService productService, ICategoryService categoryService)
    {
        _productService = productService;
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string companyId, [FromQuery] string query = "", [FromQuery] int page = 1)
    {
        int pageSize = 12;
        var productos = await _productService.GetProductosAsync(companyId, query, page, pageSize);
        var totalProductos = await _productService.GetTotalProductosAsync(companyId, query);
        var totalPages = (int)Math.Ceiling((double)totalProductos / pageSize);

        var paginationInfo = new PaginationInfo
        {
            CurrentPage = page,
            TotalPages = totalPages
        };

        paginationInfo.GeneratePageLinks();

        var result = new PaginationResult<Product>
        {
            Pagination = paginationInfo,
            Data = productos
        };

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Show(string companyId, string id)
    {
        var product = await _productService.GetByIdAsync(companyId, id);
        return Ok(product);
    }

    [HttpGet("Lista")]
    public async Task<IActionResult> Lista(string companyId, [FromQuery] string query = "")
    {
        var productos = await _productService.GetListAsync(companyId, query);
        return Ok(productos);
    }

    [HttpGet("Select2")]
    public async Task<IActionResult> Select2(string companyId, [FromQuery] string term = "")
    {
        string[] fieldNames = new string[] { "Barcode", "Description" };
        var products = await _productService.GetFilteredAsync(companyId, fieldNames, term, 6);
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

    [HttpDelete("{id}"), CustomerAuthorize(UserRole = UserRoleHelper.Admin)]
    public async Task<IActionResult> Delete(string companyId, string id)
    {
        var product = await _productService.GetByIdAsync(companyId, id);
        await _productService.RemoveAsync(companyId, product.Id);
        return Ok(product);
    }

    [HttpGet("PlantillaExcel")]
    public IActionResult PlantillaExcel()
    {
        var plantillaExcel = new ExcelPlantillaProductos().GenerarArchivo();
        // Configuramos la respuesta HTTP.
        var stream = new MemoryStream();
        plantillaExcel.SaveAs(stream);
        stream.Seek(0, SeekOrigin.Begin);
        return File(stream, ContentTypeFormat.Excel, "plantilla.xlsx");
    }

    [HttpPost("CargarProductos"), CustomerAuthorize(UserRole = UserRoleHelper.Admin)]
    public async Task<IActionResult> CargarProductosAsync(string companyId, IFormFile datos)
    {
        try
        {
            if (datos == null || datos.Length == 0)
            {
                return BadRequest("Archivo no proporcionado o vacío.");
            }

            var category = new Category() { CompanyId = companyId.Trim(), Name = "SIN CATEGORÍA" };
            category = await _categoryService.CreateAsync(category);

            var productos = new ExcelProductReader(datos, companyId, $"{category.Id}:{category.Name}").ReadProducts();
            const int batchSize = 1000;
            for (int i = 0; i < productos.Count; i += batchSize)
            {
                var batch = productos.GetRange(i, Math.Min(batchSize, productos.Count - i));
                await _productService.InsertManyAsync(batch);
            }

            return StatusCode(StatusCodes.Status201Created);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Error durante la carga de productos: {ex.Message}");
        }
    }
}
