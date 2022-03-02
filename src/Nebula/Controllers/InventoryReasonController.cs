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
    public class InventoryReasonController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public InventoryReasonController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("Index/{type}")]
        public async Task<IActionResult> Index(string type, [FromQuery] string query)
        {
            var result = from m in _context.InventoryReasons
                    .Where(m => m.Type.Equals(type))
                select m;
            if (!string.IsNullOrWhiteSpace(query))
                result = result.Where(m => m.Description.ToLower().Contains(query.ToLower()));
            var responseData = await result.AsNoTracking().ToListAsync();
            return Ok(responseData);
        }

        [HttpGet("Show/{id}")]
        public async Task<IActionResult> Show(int id)
        {
            var result = await _context.InventoryReasons.AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id.Equals(id));
            return Ok(result);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] InventoryReason model)
        {
            _context.InventoryReasons.Add(model);
            await _context.SaveChangesAsync();
            return Ok(new
            {
                Ok = true, Data = model,
                Msg = $"{model.Description} ha sido registrado!"
            });
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int? id, [FromBody] InventoryReason model)
        {
            if (id != model.Id) return BadRequest();
            _context.InventoryReasons.Update(model);
            await _context.SaveChangesAsync();
            return Ok(new
            {
                Ok = true, Data = model,
                Msg = $"{model.Description} ha sido actualizado!"
            });
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _context.InventoryReasons.FirstOrDefaultAsync(m => m.Id.Equals(id));
            if (result == null) return BadRequest();
            _context.InventoryReasons.Remove(result);
            await _context.SaveChangesAsync();
            return Ok(new {Ok = true, Data = result, Msg = $"{result.Description} ha sido borrado!"});
        }
    }
}
