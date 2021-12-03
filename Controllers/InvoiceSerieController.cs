using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nebula.Data;
using Nebula.Data.Models;

namespace Nebula.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceSerieController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public InvoiceSerieController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("Index")]
        public async Task<IActionResult> Index([FromQuery] string query)
        {
            var result = from m in _context.InvoiceSeries select m;
            if (!string.IsNullOrWhiteSpace(query))
                result = result.Where(m => m.Name.ToLower().Contains(query.ToLower()));
            result = result.Include(m => m.Warehouse);
            var responseData = await result.AsNoTracking().ToListAsync();
            return Ok(responseData);
        }

        [HttpGet("Show/{id}")]
        public async Task<IActionResult> Show(string id)
        {
            var result = await _context.InvoiceSeries.AsNoTracking()
                .Include(m => m.Warehouse)
                .FirstOrDefaultAsync(m => m.Id.ToString().Equals(id));
            return Ok(result);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] InvoiceSerie model)
        {
            _context.InvoiceSeries.Add(model);
            await _context.SaveChangesAsync();
            return Ok(new
            {
                Ok = true, Data = model,
                Msg = $"{model.Name} ha sido registrado!"
            });
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] InvoiceSerie model)
        {
            if (id == model.Id.ToString()) return BadRequest();
            _context.InvoiceSeries.Update(model);
            await _context.SaveChangesAsync();
            return Ok(new
            {
                Ok = true, Data = model,
                Msg = $"{model.Name} ha sido actualizado!"
            });
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _context.InvoiceSeries
                .FirstOrDefaultAsync(m => m.Id.ToString().Equals(id));
            if (result == null) return BadRequest();
            _context.InvoiceSeries.Remove(result);
            await _context.SaveChangesAsync();
            return Ok(new {Ok = true, Data = result, Msg = "La serie de facturación ha sido borrado!"});
        }
    }
}
