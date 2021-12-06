using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nebula.Data;
using Nebula.Data.Models;
using Nebula.Data.ViewModels;

namespace Nebula.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("Index")]
        public async Task<IActionResult> Index([FromQuery] string query)
        {
            var result = from m in _context.Categories select m;
            if (!string.IsNullOrWhiteSpace(query))
                result = result.Where(m => m.Name.ToLower().Contains(query.ToLower()));
            result = result.OrderByDescending(m => m.Id);
            var responseData = await result.AsNoTracking().Take(25).ToListAsync();
            return Ok(responseData);
        }

        [HttpGet("Show/{id}")]
        public async Task<IActionResult> Show(int id)
        {
            var result = await _context.Categories.AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id.Equals(id));
            return Ok(result);
        }

        [HttpGet("Select2")]
        public async Task<IActionResult> Select2([FromQuery] string term)
        {
            var result = from m in _context.Categories select m;
            if (!string.IsNullOrWhiteSpace(term))
                result = result.Where(m => m.Name.ToLower().Contains(term.ToLower()));
            result = result.OrderByDescending(m => m.Id);
            var responseData = await result.AsNoTracking().Take(10).ToListAsync();
            var data = new List<Select2>();
            responseData.ForEach(item =>
            {
                data.Add(new Select2()
                {
                    Id = item.Id, Text = item.Name
                });
            });
            return Ok(new {Results = data});
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] Category model)
        {
            model.Name = model.Name.ToUpper();
            _context.Categories.Add(model);
            await _context.SaveChangesAsync();
            return Ok(new
            {
                Ok = true, Data = model,
                Msg = $"{model.Name} ha sido registrado!"
            });
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Category model)
        {
            if (id != model.Id) return BadRequest();
            _context.Categories.Update(model);
            await _context.SaveChangesAsync();
            return Ok(new
            {
                Ok = true, Data = model,
                Msg = $"{model.Name} ha sido actualizado!"
            });
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _context.Categories.FirstOrDefaultAsync(m => m.Id.Equals(id));
            if (result == null) return BadRequest();
            _context.Categories.Remove(result);
            await _context.SaveChangesAsync();
            return Ok(new {Ok = true, Data = result, Msg = "La categoría ha sido borrado!"});
        }
    }
}
