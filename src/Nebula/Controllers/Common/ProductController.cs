using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Inventory.Stock;
using Nebula.Modules.Products.Models;
using Nebula.Modules.Products;
using Nebula.Modules.Configurations;
using Nebula.Modules.Auth.Helpers;
using Nebula.Modules.Sales.Helpers;
using Nebula.Modules.Products.Dto;

namespace Nebula.Controllers.Common;

[Authorize(Roles = AuthRoles.User)]
[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IProductService _productService;
    private readonly IProductStockService _productStockService;
    private readonly IConfigurationService _configurationService;

    public ProductController(IConfiguration configuration,
        IProductService productService,
        IProductStockService productStockService,
        IConfigurationService configurationService)
    {
        _configuration = configuration;
        _productService = productService;
        _productStockService = productStockService;
        _configurationService = configurationService;
    }

    public class FormData : Product
    {
        public IFormFile? File { get; set; }
    }

    [HttpGet("Index")]
    public async Task<IActionResult> Index([FromQuery] string? query)
    {
        var products = await _productService.GetListAsync(query);
        return Ok(products);
    }

    [HttpGet("Show/{id}")]
    public async Task<IActionResult> Show(string id)
    {
        var product = await _productService.GetByIdAsync(id);
        return Ok(product);
    }

    [HttpGet("Select2")]
    public async Task<IActionResult> Select2([FromQuery] string? term)
    {
        var products = await _productService.GetListAsync(term, 10);
        var data = new List<ProductSelect>();
        products.ForEach(item =>
        {
            data.Add(new ProductSelect()
            {
                Id = item.Id,
                Text = $"{item.Description} | {Convert.ToDecimal(item.PrecioVentaUnitario):N2}",
                Description = item.Description,
                Barcode = item.Barcode,
                CodProductoSUNAT = item.CodProductoSUNAT,
                PrecioVentaUnitario = item.PrecioVentaUnitario,
                IgvSunat = item.IgvSunat,
                Icbper = item.Icbper,
                ControlStock = item.ControlStock,
            });
        });
        return Ok(new { Results = data });
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromForm] FormData model)
    {
        if (model.File?.Length > 0)
        {
            var storagePath = _configuration.GetValue<string>("StoragePath");
            var dirPath = Path.Combine(storagePath, "uploads");
            var fileName = Guid.NewGuid() + model.File.FileName;
            var filePath = Path.Combine(dirPath, fileName);
            await using var stream = System.IO.File.Create(filePath);
            await model.File.CopyToAsync(stream);
            model.PathImage = fileName;
        }
        else
        {
            model.PathImage = "default.jpg";
        }

        var configuration = await _configurationService.GetAsync();
        decimal porcentajeIgv = (configuration.PorcentajeIgv / 100) + 1;
        decimal porcentajeTributo = model.IgvSunat == TipoIGV.Gravado ? porcentajeIgv : 1;
        model.ValorUnitario = model.PrecioVentaUnitario / porcentajeTributo;

        Product product = new Product()
        {
            Description = model.Description.ToUpper(),
            IgvSunat = model.IgvSunat,
            Icbper = model.Icbper,
            ValorUnitario = model.ValorUnitario,
            PrecioVentaUnitario = model.PrecioVentaUnitario,
            Barcode = model.Barcode.ToUpper(),
            CodProductoSUNAT = model.CodProductoSUNAT.ToUpper(),
            Type = model.Type,
            UndMedida = model.UndMedida,
            Category = model.Category,
            ControlStock = model.ControlStock,
            PathImage = model.PathImage,
            HasLotes = false
        };
        await _productService.CreateAsync(product);

        return Ok(product);
    }

    [HttpPut("Update/{id}")]
    public async Task<IActionResult> Update(string id, [FromForm] FormData model)
    {
        if (id != model.Id) return BadRequest();
        var product = await _productService.GetByIdAsync(id);

        if (model.File?.Length > 0)
        {
            var storagePath = _configuration.GetValue<string>("StoragePath");
            var dirPath = Path.Combine(storagePath, "uploads");
            // borrar archivo antiguo si existe.
            var oldFile = Path.Combine(dirPath, product.PathImage ?? string.Empty);
            if (product.PathImage != null && !product.PathImage.Equals("default.jpg"))
                if (System.IO.File.Exists(oldFile))
                    System.IO.File.Delete(oldFile);
            // copiar nuevo archivo.
            var fileName = Guid.NewGuid() + model.File.FileName;
            var filePath = Path.Combine(dirPath, fileName);
            await using var stream = System.IO.File.Create(filePath);
            await model.File.CopyToAsync(stream);
            model.PathImage = fileName;
        }
        else
        {
            model.PathImage = product.PathImage;
        }

        var configuration = await _configurationService.GetAsync();
        decimal porcentajeIgv = (configuration.PorcentajeIgv / 100) + 1;
        decimal porcentajeTributo = model.IgvSunat == TipoIGV.Gravado ? porcentajeIgv : 1;
        model.ValorUnitario = model.PrecioVentaUnitario / porcentajeTributo;
        // actualización de datos del modelo.
        product.Description = model.Description.ToUpper();
        product.IgvSunat = model.IgvSunat;
        product.Icbper = model.Icbper;
        product.ValorUnitario = model.ValorUnitario;
        product.PrecioVentaUnitario = model.PrecioVentaUnitario;
        product.Barcode = model.Barcode.ToUpper();
        product.CodProductoSUNAT = model.CodProductoSUNAT.ToUpper();
        product.Type = model.Type;
        product.UndMedida = model.UndMedida;
        product.Category = model.Category;
        product.ControlStock = model.ControlStock;
        product.PathImage = model.PathImage;
        product.HasLotes = product.HasLotes;
        await _productService.UpdateAsync(id, product);

        return Ok(product);
    }

    [HttpDelete("Delete/{id}"), Authorize(Roles = AuthRoles.Admin)]
    public async Task<IActionResult> Delete(string id)
    {
        var product = await _productService.GetByIdAsync(id);
        // directorio principal.
        var storagePath = _configuration.GetValue<string>("StoragePath");
        var dirPath = Path.Combine(storagePath, "uploads");
        // borrar archivo si existe.
        if (product.PathImage != "default.jpg")
        {
            var file = Path.Combine(dirPath, product.PathImage);
            if (System.IO.File.Exists(file)) System.IO.File.Delete(file);
        }

        // borrar registro.
        await _productService.RemoveAsync(id);
        return Ok(product);
    }

    [Obsolete]
    [HttpGet("Stock/{id}")]
    public async Task<IActionResult> Stock(string id)
    {
        var productStocks = await _productStockService.GetProductStockReportAsync(id);
        return Ok(productStocks);
    }

}
