using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nebula.Data;
using Nebula.Data.Models;

namespace Nebula.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CompanyController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("Show")]
        public async Task<IActionResult> Show()
        {
            if (await _context.Company.AsNoTracking().CountAsync() == 0)
            {
                _context.Company.Add(new Company());
                await _context.SaveChangesAsync();
            }

            var result = await _context.Company.FirstAsync();
            return Ok(result);
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int? id, [FromBody] Company model)
        {
            if (id != model.Id) return BadRequest();
            _context.Company.Update(model);
            await _context.SaveChangesAsync();
            return Ok(new
            {
                Ok = true, Data = model,
                Msg = $"{model.Ruc}, ha sido actualizado!"
            });
        }
    }
}
