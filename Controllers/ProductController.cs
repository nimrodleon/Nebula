using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nebula.Data;
using Nebula.Data.Models;

namespace Nebula.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("Show/{id}")]
        public async Task<IActionResult> Show(int? id)
        {
            if (id == null) return BadRequest();
            var result = await _context.Products.IgnoreQueryFilters()
                .AsNoTracking().FirstOrDefaultAsync(m => m.Id.Equals(id));
            if (result == null) return BadRequest();
            return Ok(result);
        }

        [HttpPost("Store")]
        public async Task<IActionResult> Store([FromForm] Product model)
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

            _context.Products.Add(model);
            await _context.SaveChangesAsync();
            return Ok(new
            {
                Ok = true, Data = model,
                Msg = $"{model.Description} ha sido registrado!"
            });
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int? id, [FromForm] Product model)
        {
            if (id != model.Id) return BadRequest();
            if (model.File?.Length > 0)
            {
                var product = await _context.Products
                    .AsNoTracking().SingleAsync(m => m.Id.Equals(id));
                if (product == null) return BadRequest();
                // directorio principal.
                var dirPath = Path.Combine(Environment
                    .GetFolderPath(Environment.SpecialFolder.UserProfile), "StaticFiles");
                // borrar archivo antiguo si existe.
                var oldFile = Path.Combine(dirPath, product.PathImage);
                if (System.IO.File.Exists(oldFile)) System.IO.File.Delete(oldFile);
                // copiar nuevo archivo.
                var fileName = Guid.NewGuid() + model.File.FileName;
                var filePath = Path.Combine(dirPath, fileName);
                await using var stream = System.IO.File.Create(filePath);
                await model.File.CopyToAsync(stream);
                model.PathImage = fileName;
            }

            _context.Products.Update(model);
            await _context.SaveChangesAsync();
            return Ok(new
            {
                Ok = true, Data = model,
                Msg = $"{model.Description} ha sido actualizado!"
            });
        }

        [HttpDelete("Destroy/{id}")]
        public async Task<IActionResult> Destroy(int? id)
        {
            var result = await _context.Products
                .FirstOrDefaultAsync(m => m.Id.Equals(id));
            if (result == null) return BadRequest();
            // directorio principal.
            var dirPath = Path.Combine(Environment
                .GetFolderPath(Environment.SpecialFolder.UserProfile), "StaticFiles");
            // borrar archivo si existe.
            var file = Path.Combine(dirPath, result.PathImage);
            if (System.IO.File.Exists(file)) System.IO.File.Delete(file);
            // borrar registro.
            _context.Products.Remove(result);
            await _context.SaveChangesAsync();
            return Ok(new { Ok = true, Data = result, Msg = "El producto ha sido borrado!" });
        }
    }
}
