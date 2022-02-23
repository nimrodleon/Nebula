using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nebula.Data;
using Nebula.Data.Models;
using Nebula.Data.ViewModels;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;

namespace Nebula.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IRavenDbContext _context;

        public ProductController(IRavenDbContext context)
        {
            _context = context;
        }

        [HttpGet("Index")]
        public async Task<IActionResult> Index([FromQuery] string query)
        {
            using var session = _context.Store.OpenAsyncSession();
            IRavenQueryable<Product> products = from m in session.Query<Product>() select m;
            if (!string.IsNullOrWhiteSpace(query))
                products = products.Search(m => m.Description, $"*{query.ToUpper()}*");
            var responseData = await products.Take(25).ToListAsync();
            return Ok(responseData);
        }

        [HttpGet("Show/{id}")]
        public async Task<IActionResult> Show(string id)
        {
            using var session = _context.Store.OpenAsyncSession();
            Product product = await session.LoadAsync<Product>(id);
            return Ok(product);
        }

        [HttpGet("Select2")]
        public async Task<IActionResult> Select2([FromQuery] string term)
        {
            using var session = _context.Store.OpenAsyncSession();
            IRavenQueryable<Product> products = from m in session.Query<Product>() select m;
            if (!string.IsNullOrWhiteSpace(term))
                products = products.Search(m => m.Description, $"*{term.ToUpper()}*");
            var responseData = await products.Take(25).ToListAsync();
            var data = new List<Select2>();
            responseData.ForEach(item =>
            {
                data.Add(new Select2()
                {
                    Id = item.Id,
                    Text = $"{item.Description} | {Convert.ToDecimal(item.Price1):N2}"
                });
            });
            return Ok(new {Results = data});
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromForm] Product model)
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

            using var session = _context.Store.OpenAsyncSession();
            model.Id = String.Empty;
            model.Description = model.Description.ToUpper();
            await session.StoreAsync(model);
            await session.SaveChangesAsync();

            return Ok(new
            {
                Ok = true, Data = model,
                Msg = $"El producto {model.Description} ha sido registrado!"
            });
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(string id, [FromForm] Product model)
        {
            if (id != model.Id) return BadRequest();
            using var session = _context.Store.OpenAsyncSession();
            Product product = await session.LoadAsync<Product>(id);
            if (product == null) return BadRequest();

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
            await session.SaveChangesAsync();

            return Ok(new
            {
                Ok = true, Data = product,
                Msg = $"El producto {product.Description} ha sido actualizado!"
            });
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            using var session = _context.Store.OpenAsyncSession();
            Product product = await session.LoadAsync<Product>(id);
            if (product == null) return BadRequest();
            // directorio principal.
            var dirPath = Path.Combine(Environment
                .GetFolderPath(Environment.SpecialFolder.UserProfile), "StaticFiles");
            // borrar archivo si existe.
            var file = Path.Combine(dirPath, product.PathImage);
            if (System.IO.File.Exists(file)) System.IO.File.Delete(file);
            // borrar registro.
            session.Delete(product);
            await session.SaveChangesAsync();
            return Ok(new {Ok = true, Data = product, Msg = "El producto ha sido borrado!"});
        }
    }
}
