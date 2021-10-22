using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nebula.Data;
using Nebula.Data.Models;

namespace Nebula.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleDocTypeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PeopleDocTypeController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            var result = await _context.PeopleDocTypes.ToListAsync();
            return Ok(result);
        }

        [HttpGet("Show/{id}")]
        public async Task<IActionResult> Show(string id)
        {
            if (id == null) return BadRequest();
            var result = await _context.PeopleDocTypes.IgnoreQueryFilters()
                .FirstOrDefaultAsync(m => m.Id.ToString().Equals(id));
            if (result == null) return BadRequest();
            return Ok(result);
        }

        [HttpPost("Store")]
        public async Task<IActionResult> Store([FromBody] PeopleDocType model)
        {
            _context.PeopleDocTypes.Add(model);
            await _context.SaveChangesAsync();
            return Ok(new { Ok = true, PeopleDocType = model });
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] PeopleDocType model)
        {
            if (id != model.Id.ToString()) return BadRequest();
            _context.PeopleDocTypes.Update(model);
            await _context.SaveChangesAsync();
            return Ok(new { Ok = true, PeopleDocType = model });
        }

        [HttpDelete("Destroy/{id}")]
        public async Task<IActionResult> Destroy(string id)
        {
            var result = await _context.PeopleDocTypes
                .FirstOrDefaultAsync(m => m.Id.ToString().Equals(id));
            if (result == null) return BadRequest();
            _context.PeopleDocTypes.Remove(result);
            await _context.SaveChangesAsync();
            return Ok(new { Ok = true, Msg = "Tipo documento borrado!" });
        }
    }
}
