using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    }
}
