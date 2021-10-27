using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nebula.Data;
using Nebula.Data.Models;

namespace Nebula.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CajaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CajaController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            var result = await _context.Cajas.AsNoTracking().ToListAsync();
            return Ok(result);
        }

        [HttpGet("Show/{id}")]
        public async Task<IActionResult> Show(string id)
        {
            if (id == null) return BadRequest();
            var result = await _context.Cajas.IgnoreQueryFilters()
                .AsNoTracking().FirstOrDefaultAsync(m => m.Id.ToString().Equals(id));
            if (result == null) return BadRequest();
            return Ok(result);
        }

        [HttpPost("Store")]
        public async Task<IActionResult> Store([FromBody] Caja model)
        {
            _context.Cajas.Add(model);
            await _context.SaveChangesAsync();
            return Ok(new
            {
                Ok = true, Data = model,
                Msg = $"{model.Name} ha sido registrado!"
            });
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] Caja model)
        {
            if (id != model.Id.ToString()) return BadRequest();
            _context.Cajas.Update(model);
            await _context.SaveChangesAsync();
            return Ok(new
            {
                Ok = true, Data = model,
                Msg = $"{model.Name} ha sido actualizado!"
            });
        }

        [HttpDelete("Destroy/{id}")]
        public async Task<IActionResult> Destroy(string id)
        {
            var result = await _context.Cajas.FirstOrDefaultAsync(m => m.Id.ToString().Equals(id));
            if (result == null) return BadRequest();
            _context.Cajas.Remove(result);
            await _context.SaveChangesAsync();
            return Ok(new { Ok = true, Data = result, Msg = "La caja ha sido borrado!" });
        }
    }
}
