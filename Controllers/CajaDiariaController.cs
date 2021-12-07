using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nebula.Data;
using Nebula.Data.Models;
using Nebula.Data.ViewModels;

namespace Nebula.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CajaDiariaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CajaDiariaController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("Index")]
        public async Task<IActionResult> Index([FromQuery] DateQuery model)
        {
            var result = await _context.CajasDiaria.Where(m =>
                    m.Year.Equals(model.Year) && m.Month.Equals(model.Month))
                .OrderByDescending(m => m.Id).AsNoTracking().ToListAsync();
            return Ok(result);
        }

        [HttpGet("Show/{id}")]
        public async Task<IActionResult> Show(int id)
        {
            var result = await _context.CajasDiaria.AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id.Equals(id));
            if (result == null) return BadRequest();
            return Ok(result);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] AperturaCaja model)
        {
            var invoiceSerie = await _context.InvoiceSeries.AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id.ToString().Equals(model.SerieId));
            if (invoiceSerie == null) return BadRequest();

            await using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var cajaDiaria = new CajaDiaria()
                    {
                        InvoiceSerie = invoiceSerie,
                        Name = invoiceSerie.Name,
                        StartDate = DateTime.Now,
                        State = "ABIERTO",
                        TotalApertura = model.Total,
                        TotalContabilizado = 0.0M,
                        TotalCierre = 0.0M,
                        Year = DateTime.Now.ToString("yyyy"),
                        Month = DateTime.Now.ToString("MM"),
                    };
                    _context.CajasDiaria.Add(cajaDiaria);
                    await _context.SaveChangesAsync();

                    // registrar apertura de caja en detalle de caja diaria.
                    var detalleCaja = new CashierDetail()
                    {
                        CajaDiaria = cajaDiaria,
                        TypeOperation = TypeOperation.CajaChica,
                        StartDate = DateTime.Now,
                        Document = "-",
                        Contact = "-",
                        Glosa = "APERTURA DE CAJA",
                        Type = "ENTRADA",
                        Total = model.Total
                    };
                    _context.CashierDetails.Add(detalleCaja);
                    await _context.SaveChangesAsync();

                    // confirmar transacción.
                    await transaction.CommitAsync();

                    return Ok(new
                    {
                        Ok = true, Data = cajaDiaria,
                        Msg = "La apertura de caja ha sido registrado!"
                    });
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                }
            }

            return BadRequest(new
            {
                Ok = true, Msg = "Hubo un error, al registrar la apertura de caja!"
            });
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CerrarCaja model)
        {
            var result = await _context.CajasDiaria.FirstOrDefaultAsync(m => m.Id.Equals(id));
            if (result == null) return BadRequest();
            result.TotalContabilizado = model.TotalContabilizado;
            result.TotalCierre = model.TotalCierre;
            result.State = "CERRADO";
            _context.CajasDiaria.Update(result);
            await _context.SaveChangesAsync();
            return Ok(new
            {
                Ok = true, Data = model,
                Msg = "El cierre de caja ha sido registrado!"
            });
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _context.CajasDiaria.FirstOrDefaultAsync(m => m.Id.Equals(id));
            if (result == null) return BadRequest();
            _context.CajasDiaria.Remove(result);
            await _context.SaveChangesAsync();
            return Ok(new {Ok = true, Data = result, Msg = "La caja diaria ha sido borrado!"});
        }
    }
}
