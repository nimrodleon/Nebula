using Microsoft.AspNetCore.Mvc;
using Nebula.Data.Models;
using Nebula.Data.Services;
using Nebula.Data.ViewModels;

namespace Nebula.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly ProductService _productService;

    public ProductController(ProductService productService) =>
        _productService = productService;

    public class FormData : Product
    {
        public IFormFile? File { get; set; }
    }

    [HttpGet("Index")]
    public async Task<IActionResult> Index([FromQuery] string? query)
    {
        var responseData = await _productService.GetListAsync(query);
        return Ok(responseData);
    }

    [HttpGet("Show/{id}")]
    public async Task<IActionResult> Show(string id)
    {
        var product = await _productService.GetAsync(id);
        return Ok(product);
    }

    [HttpGet("Select2")]
    public async Task<IActionResult> Select2([FromQuery] string term)
    {
        var responseData = await _productService.GetListAsync(term, 10);
        var data = new List<Select2>();
        responseData.ForEach(item =>
        {
            data.Add(new Select2()
            {
                Id = item.Id,
                Text = $"{item.Description} | {Convert.ToDecimal(item.Price1):N2}"
            });
        });
        return Ok(new { Results = data });
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromForm] FormData model)
    {
        if (model.File?.Length > 0)
        {
            var dirPath = Path.Combine(Environment
                .GetFolderPath(Environment.SpecialFolder.UserProfile), "StaticFiles");
            var fileName = Guid.NewGuid() + model.File.FileName;
            var filePath = Path.Combine(dirPath, fileName);
            await using var stream = System.IO.File.Create(filePath);
            await model.File.CopyToAsync(stream);
            model.PathImage = fileName;
        }
        else
        {
            model.PathImage = "default.png";
        }

        Product product = new Product()
        {
            Description = model.Description.ToUpper(),
            Barcode = model.Barcode,
            Price1 = model.Price1,
            Price2 = model.Price2,
            FromQty = model.FromQty,
            IgvSunat = model.IgvSunat,
            Icbper = model.Icbper,
            Category = model.Category,
            UndMedida = model.UndMedida,
            Type = model.Type,
            PathImage = model.PathImage
        };
        await _productService.CreateAsync(product);

        return Ok(new
        {
            Ok = true,
            Data = model,
            Msg = $"El producto {model.Description} ha sido registrado!"
        });
    }

    [HttpPut("Update/{id}")]
    public async Task<IActionResult> Update(string id, [FromForm] FormData model)
    {
        if (id != model.Id) return BadRequest();
        var product = await _productService.GetAsync(id);

        if (model.File?.Length > 0)
        {
            var dirPath = Path.Combine(Environment
                .GetFolderPath(Environment.SpecialFolder.UserProfile), "StaticFiles");
            // borrar archivo antiguo si existe.
            var oldFile = Path.Combine(dirPath, product.PathImage ?? string.Empty);
            if (product.PathImage != null && !product.PathImage.Equals("default.png"))
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

        product.Description = model.Description.ToUpper();
        product.Barcode = model.Barcode;
        product.Price1 = model.Price1;
        product.Price2 = model.Price2;
        product.FromQty = model.FromQty;
        product.IgvSunat = model.IgvSunat;
        product.Icbper = model.Icbper;
        product.Category = model.Category;
        product.UndMedida = model.UndMedida;
        product.Type = model.Type;
        product.PathImage = model.PathImage;
        await _productService.UpdateAsync(id, product);

        return Ok(new
        {
            Ok = true,
            Data = model,
            Msg = $"El producto {model.Description} ha sido actualizado!"
        });
    }

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var product = await _productService.GetAsync(id);
        // directorio principal.
        var dirPath = Path.Combine(Environment
            .GetFolderPath(Environment.SpecialFolder.UserProfile), "StaticFiles");
        // borrar archivo si existe.
        var file = Path.Combine(dirPath, product.PathImage);
        if (System.IO.File.Exists(file)) System.IO.File.Delete(file);
        // borrar registro.
        await _productService.RemoveAsync(id);
        return Ok(new { Ok = true, Data = product, Msg = "El producto ha sido borrado!" });
    }
}
