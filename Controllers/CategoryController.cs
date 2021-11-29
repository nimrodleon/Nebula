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

        [HttpPost("Store")]
        public async Task<IActionResult> Store([FromBody] Category model)
        {
            _context.Categories.Add(model);
            await _context.SaveChangesAsync();
            return Ok(new
            {
                Ok = true, Data = model,
                Msg = $"{model.Name} ha sido registrado!"
            });
        }
    }
}
