using System;
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
    public class CashierDetailController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CashierDetailController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("Index/{id}")]
        public async Task<IActionResult> Index(int id, [FromQuery] string query)
        {
            var result = from m in _context.CashierDetails
                where m.CajaDiariaId.Equals(id)
                select m;
            if (!string.IsNullOrWhiteSpace(query))
                result = result.Where(m =>
                    m.Document.Contains(query) || m.Contact.ToLower().Contains(query.ToLower()) ||
                    m.Glosa.ToLower().Contains(query.ToLower()));
            result = result.OrderByDescending(m => m.Id);
            var responseData = await result.AsNoTracking().ToListAsync();
            return Ok(responseData);
        }

        [HttpPost("Store")]
        public async Task<IActionResult> Store([FromBody] CashierDetail model)
        {
            if (model.Type.Equals("Egreso"))
                model.Total = model.Total * (0 - 1);
            model.TypeOperation = TypeOperation.CajaChica;
            model.StartDate = DateTime.Now;
            _context.CashierDetails.Add(model);
            await _context.SaveChangesAsync();
            return Ok(new
            {
                Ok = true, Data = model,
                Msg = "La operación ha sido registrado!"
            });
        }
    }
}
