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
    public class UndMedidaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UndMedidaController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("Index")]
        public async Task<IActionResult> Index([FromQuery] string query)
        {
            var result = from m in _context.UndMedida select m;
            if (!string.IsNullOrWhiteSpace(query))
                result = result.Where(m => m.Name.ToLower().Contains(query.ToLower()));
            result = result.OrderByDescending(m => m.Id);
            var responseData = await result.AsNoTracking().Take(25).ToListAsync();
            return Ok(responseData);
        }

        [HttpGet("Show/{id}")]
        public async Task<IActionResult> Show(string id)
        {
            if (id == null) return BadRequest();
            var result = await _context.UndMedida.IgnoreQueryFilters()
                .AsNoTracking().FirstOrDefaultAsync(m => m.Id.ToString().Equals(id));
            if (result == null) return BadRequest();
            return Ok(result);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] UndMedida model)
        {
            _context.UndMedida.Add(model);
            await _context.SaveChangesAsync();
            return Ok(new
            {
                Ok = true, Data = model,
                Msg = $"{model.Name} ha sido registrado!"
            });
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UndMedida model)
        {
            if (id != model.Id.ToString()) return BadRequest();
            _context.UndMedida.Update(model);
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
            var result = await _context.UndMedida
                .FirstOrDefaultAsync(m => m.Id.ToString().Equals(id));
            if (result == null) return BadRequest();
            _context.UndMedida.Remove(result);
            await _context.SaveChangesAsync();
            return Ok(new {Ok = true, Data = result, Msg = "La UndMedida ha sido borrado!"});
        }
    }
}
