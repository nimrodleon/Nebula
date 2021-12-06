using System.Linq;
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
        public async Task<IActionResult> Index([FromQuery] string query)
        {
            var result = from m in _context.PeopleDocTypes select m;
            if (!string.IsNullOrWhiteSpace(query))
                result = result.Where(m => m.Description.ToLower().Contains(query.ToLower()));
            result = result.OrderByDescending(m => m.Id);
            var responseData = await result.AsNoTracking().Take(25).ToListAsync();
            return Ok(responseData);
        }

        [HttpGet("Show/{id}")]
        public async Task<IActionResult> Show(string id)
        {
            if (id == null) return BadRequest();
            var result = await _context.PeopleDocTypes.IgnoreQueryFilters()
                .AsNoTracking().FirstOrDefaultAsync(m => m.Id.ToString().Equals(id));
            if (result == null) return BadRequest();
            return Ok(result);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] PeopleDocType model)
        {
            _context.PeopleDocTypes.Add(model);
            await _context.SaveChangesAsync();
            return Ok(new
            {
                Ok = true, Data = model,
                Msg = $"{model.Description} ha sido registrado!"
            });
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] PeopleDocType model)
        {
            if (id != model.Id.ToString()) return BadRequest();
            _context.PeopleDocTypes.Update(model);
            await _context.SaveChangesAsync();
            return Ok(new
            {
                Ok = true, Data = model,
                Msg = $"{model.Description} ha sido actualizado!"
            });
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _context.PeopleDocTypes
                .FirstOrDefaultAsync(m => m.Id.ToString().Equals(id));
            if (result == null) return BadRequest();
            _context.PeopleDocTypes.Remove(result);
            await _context.SaveChangesAsync();
            return Ok(new {Ok = true, Data = result, Msg = "El tipo documento ha sido borrado!"});
        }
    }
}
