using Microsoft.AspNetCore.Mvc;
using Nebula.Data;
using Nebula.Data.Models;
using Nebula.Data.ViewModels;
using Raven.Client.Documents;

namespace Nebula.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CajaDiariaController : ControllerBase
    {
        private readonly IRavenDbContext _context;

        public CajaDiariaController(IRavenDbContext context)
        {
            _context = context;
        }

        [HttpGet("Index")]
        public async Task<IActionResult> Index([FromQuery] DateQuery model)
        {
            using var session = _context.Store.OpenAsyncSession();
            List<CajaDiaria> cajaDiarias = await session.Query<CajaDiaria>()
                .Where(m => m.Year == model.Year && m.Month == model.Month)
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();
            return Ok(cajaDiarias);
        }

        [HttpGet("Show/{id}")]
        public async Task<IActionResult> Show(string id)
        {
            using var session = _context.Store.OpenAsyncSession();
            CajaDiaria cajaDiaria = await session.LoadAsync<CajaDiaria>(id);
            return Ok(cajaDiaria);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] AperturaCaja model)
        {
            using var session = _context.Store.OpenAsyncSession();
            InvoiceSerie invoiceSerie = await session.LoadAsync<InvoiceSerie>(model.SerieId);
            if (invoiceSerie == null) return BadRequest();

            var cajaDiaria = new CajaDiaria()
            {
                Id = string.Empty,
                Terminal = $"{invoiceSerie.Id}:{invoiceSerie.Name}",
                Status = "ABIERTO",
                TotalApertura = model.Total,
                TotalContabilizado = 0.0M,
                TotalCierre = 0.0M,
                Turno = model.Turno
            };
            await session.StoreAsync(cajaDiaria);
            await session.SaveChangesAsync();

            // registrar apertura de caja.
            var detalleCaja = new CashierDetail()
            {
                Id = string.Empty,
                CajaDiaria = cajaDiaria.Id,
                Remark = "APERTURA DE CAJA",
                Type = "ENTRADA",
                TypeOperation = TypeOperation.CajaChica,
                FormaPago = "Contado",
                Amount = model.Total
            };
            await session.StoreAsync(detalleCaja);
            await session.SaveChangesAsync();

            return Ok(new
            {
                Ok = true, Data = cajaDiaria,
                Msg = "La apertura de caja ha sido registrado!"
            });
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] CerrarCaja model)
        {
            using var session = _context.Store.OpenAsyncSession();
            CajaDiaria cajaDiaria = await session.LoadAsync<CajaDiaria>(id);
            if (cajaDiaria == null) return BadRequest();
            cajaDiaria.TotalContabilizado = model.TotalContabilizado;
            cajaDiaria.TotalCierre = model.TotalCierre;
            cajaDiaria.Status = "CERRADO";
            await session.SaveChangesAsync();
            return Ok(new
            {
                Ok = true, Data = cajaDiaria,
                Msg = "El cierre de caja ha sido registrado!"
            });
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            using var session = _context.Store.OpenAsyncSession();
            CajaDiaria cajaDiaria = await session.LoadAsync<CajaDiaria>(id);
            session.Delete(cajaDiaria);
            await session.SaveChangesAsync();
            return Ok(new {Ok = true, Data = cajaDiaria, Msg = "La caja diaria ha sido borrado!"});
        }
    }
}
