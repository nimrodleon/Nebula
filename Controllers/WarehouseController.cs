using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nebula.Data;
using Nebula.Data.Models;

namespace Nebula.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarehouseController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public WarehouseController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            var result = await _context.Warehouses.AsNoTracking().ToListAsync();
            return Ok(result);
        }

        [HttpPost("Store")]
        public async Task<IActionResult> Store([FromBody] Warehouse model)
        {
            _context.Warehouses.Add(model);
            await _context.SaveChangesAsync();
            return Ok(new
            {
                Ok = true, Data = model,
                Msg = $"{model.Name} ha sido registrado!"
            });
        }
    }
}
