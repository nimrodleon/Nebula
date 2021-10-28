using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nebula.Data;
using Nebula.Data.Helpers;
using Nebula.Data.Models;
using Nebula.Data.Services;

namespace Nebula.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IUriService _uriService;

        public ProductController(ApplicationDbContext context, IUriService uriService)
        {
            _context = context;
            _uriService = uriService;
        }

        [HttpGet("Index")]
        public async Task<IActionResult> Index([FromQuery] PaginationFilter filter)
        {
            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize, filter.Query);
            var skip = (validFilter.PageNumber - 1) * validFilter.PageSize;
            var result = from p in _context.Products select p;
            if (!string.IsNullOrWhiteSpace(filter.Query))
                result = result.Where(m => m.Barcode.Contains(filter.Query)
                                           || m.Description.ToLower().Contains(filter.Query.ToLower()));
            result = result.OrderByDescending(m => m.Id);
            var pagedData = await result.AsNoTracking().Skip(skip).Take(validFilter.PageSize).ToListAsync();
            var totalRecords = await result.CountAsync();
            var pagedResponse =
                PaginationHelper.CreatePagedResponse(pagedData, validFilter, totalRecords, _uriService, route);
            return Ok(pagedResponse);
        }

        [HttpGet("Terminal")]
        public async Task<IActionResult> Terminal([FromQuery] string query)
        {
            var result = from p in _context.Products select p;
            if (!string.IsNullOrWhiteSpace(query))
                result = result.Where(m => m.Description.ToLower().Contains(query.ToLower()));
            result = result.OrderByDescending(m => m.Id);
            var responseData = await result.AsNoTracking().Take(25).ToListAsync();
            return Ok(responseData);
        }

        [HttpGet("Show/{id}")]
        public async Task<IActionResult> Show(int? id)
        {
            if (id == null) return BadRequest();
            var result = await _context.Products.AsNoTracking()
                .IgnoreQueryFilters().FirstOrDefaultAsync(m => m.Id.Equals(id));
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
            else
            {
                model.PathImage = "default.png";
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
            var product = await _context.Products
                .AsNoTracking().SingleAsync(m => m.Id.Equals(id));
            if (product == null) return BadRequest();

            if (model.File?.Length > 0)
            {
                var dirPath = Path.Combine(Environment
                    .GetFolderPath(Environment.SpecialFolder.UserProfile), "StaticFiles");
                // borrar archivo antiguo si existe.
                var oldFile = Path.Combine(dirPath, product.PathImage ?? string.Empty);
                if (System.IO.File.Exists(oldFile)) System.IO.File.Delete(oldFile);
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
