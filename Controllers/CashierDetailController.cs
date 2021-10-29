using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nebula.Data;

namespace Nebula.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CashierDetailController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CashierDetailController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("Index/{id}")]
        public async Task<IActionResult> Index(int? id, [FromQuery] string query)
        {
            if (id == null) return BadRequest();
            var result = from m in _context.CashierDetails
                where m.CajaDiariaId.Equals(id)
                select m;
            if (!string.IsNullOrWhiteSpace(query))
                result = result.Where(m => m.Document.Contains(query) || m.Contact.ToLower().Contains(query));
            var responseData = await result.AsNoTracking().ToListAsync();
            return Ok(responseData);
        }
    }
}
