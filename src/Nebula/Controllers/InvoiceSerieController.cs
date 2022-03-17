using Microsoft.AspNetCore.Mvc;
using Nebula.Data;
using Nebula.Data.Models;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;

namespace Nebula.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceSerieController : ControllerBase
    {
        private readonly IRavenDbContext _context;

        public InvoiceSerieController(IRavenDbContext context)
        {
            _context = context;
        }

        [HttpGet("Index")]
        public async Task<IActionResult> Index([FromQuery] string? query)
        {
            using var session = _context.Store.OpenAsyncSession();
            IRavenQueryable<InvoiceSerie> series = from m in session.Query<InvoiceSerie>() select m;
            if (!string.IsNullOrWhiteSpace(query))
                series = series.Search(m => m.Name, $"*{query.ToUpper()}*");
            var responseData = await series.ToListAsync();
            return Ok(responseData);
        }

        [HttpGet("Show/{id}")]
        public async Task<IActionResult> Show(string id)
        {
            using var session = _context.Store.OpenAsyncSession();
            InvoiceSerie serie = await session.LoadAsync<InvoiceSerie>(id);
            return Ok(serie);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] InvoiceSerie model)
        {
            using var session = _context.Store.OpenAsyncSession();
            model.Id = string.Empty;
            model.Name = model.Name.ToUpper();
            await session.StoreAsync(model);
            await session.SaveChangesAsync();

            return Ok(new
            {
                Ok = true, Data = model,
                Msg = $"La serie {model.Name} ha sido registrado!"
            });
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] InvoiceSerie model)
        {
            if (id != model.Id) return BadRequest();
            using var session = _context.Store.OpenAsyncSession();
            InvoiceSerie serie = await session.LoadAsync<InvoiceSerie>(id);
            serie.Name = model.Name;
            serie.Warehouse = model.Warehouse;
            serie.Factura = model.Factura;
            serie.CounterFactura = model.CounterFactura;
            serie.Boleta = model.Boleta;
            serie.CounterBoleta = model.CounterBoleta;
            serie.NotaDeVenta = model.NotaDeVenta;
            serie.CounterNotaDeVenta = model.CounterNotaDeVenta;
            await session.SaveChangesAsync();

            return Ok(new
            {
                Ok = true, Data = serie,
                Msg = $"La serie {serie.Name} ha sido actualizado!"
            });
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            using var session = _context.Store.OpenAsyncSession();
            InvoiceSerie serie = await session.LoadAsync<InvoiceSerie>(id);
            session.Delete(serie);
            await session.SaveChangesAsync();
            return Ok(new {Ok = true, Data = serie, Msg = "La serie de facturación ha sido borrado!"});
        }
    }
}
