using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nebula.Data;

namespace Nebula.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SunatController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SunatController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("TypeOperation")]
        public async Task<IActionResult> TypeOperation()
        {
            var result = await _context.TypeOperationSunat.AsNoTracking().ToListAsync();
            return Ok(result);
        }
    }
}
