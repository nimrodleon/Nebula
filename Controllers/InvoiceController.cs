using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nebula.Data;
using Nebula.Data.Services;
using Nebula.Data.ViewModels;

namespace Nebula.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly ApplicationDbContext _context;
        private readonly IComprobanteService _comprobanteService;
        private readonly ICpeService _cpeService;

        public InvoiceController(ILogger<InvoiceController> logger,
            ApplicationDbContext context, IComprobanteService comprobanteService, ICpeService cpeService)
        {
            _logger = logger;
            _context = context;
            _comprobanteService = comprobanteService;
            _cpeService = cpeService;
        }

        [HttpGet("Index/{type}")]
        public async Task<IActionResult> Index(string type, [FromQuery] VoucherQuery model)
        {
            if (type == null) return BadRequest();
            var result = from m in _context.Invoices.Where(m =>
                    m.InvoiceType.Equals(type) && m.Year.Equals(model.Year) && m.Month.Equals(model.Month))
                select m;
            if (!string.IsNullOrWhiteSpace(model.Query))
                result = result.Where(m => m.RznSocialUsuario.ToUpper().Contains(model.Query.ToUpper()));
            result = result.OrderByDescending(m => m.Id);
            var responseData = await result.AsNoTracking().ToListAsync();
            return Ok(responseData);
        }

        [HttpGet("Show/{id}")]
        public async Task<IActionResult> Show(int id)
        {
            var result = await _context.Invoices.AsNoTracking()
                .Include(m => m.InvoiceDetails)
                .Include(m => m.Tributos)
                .Include(m => m.InvoiceAccounts)
                .FirstOrDefaultAsync(m => m.Id.Equals(id));
            if (result == null) return BadRequest();
            return Ok(result);
        }

        /// <summary>
        /// registrar comprobante de venta.
        /// </summary>
        /// <param name="id">ID serie de facturación</param>
        /// <param name="model">Comprobante</param>
        /// <returns>IActionResult</returns>
        [HttpPost("CreateSale/{id}")]
        public async Task<IActionResult> CreateSale(int id, [FromBody] Comprobante model)
        {
            try
            {
                _comprobanteService.SetModel(model);
                var invoice = await _comprobanteService.SaveSale(id);
                model.InvoiceId = invoice.Id;
                bool fileExist = false;

                // generar fichero JSON según tipo comprobante.
                switch (invoice.DocType)
                {
                    case "BOLETA":
                        fileExist = await _cpeService.CreateBoletaJson(invoice.Id);
                        break;
                    case "FACTURA":
                        fileExist = await _cpeService.CreateFacturaJson(invoice.Id);
                        break;
                }

                return Ok(new
                {
                    Ok = fileExist, Data = model,
                    Msg = $"{invoice.Serie} - {invoice.Number} ha sido registrado!"
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest(new {Ok = false, Msg = e.Message});
            }
        }

        /// <summary>
        /// registrar venta rápida.
        /// </summary>
        /// <param name="id">ID caja diaria</param>
        /// <param name="model">Venta</param>
        /// <returns>IActionResult</returns>
        [HttpPost("CreateQuickSale/{id}")]
        public async Task<IActionResult> CreateQuickSale(int id, [FromBody] Venta model)
        {
            try
            {
                _comprobanteService.SetModel(model);
                var invoice = await _comprobanteService.SaveQuickSale(id);
                bool fileExist = false;

                // generar fichero JSON según tipo comprobante.
                switch (invoice.DocType)
                {
                    case "BOLETA":
                        fileExist = await _cpeService.CreateBoletaJson(invoice.Id);
                        break;
                    case "FACTURA":
                        fileExist = await _cpeService.CreateFacturaJson(invoice.Id);
                        break;
                }

                // Configurar valor de retorno.
                model.InvoiceId = invoice.Id;
                model.Vuelto = model.MontoTotal - model.SumImpVenta;

                return Ok(new
                {
                    Ok = fileExist, Data = model,
                    Msg = $"{invoice.Serie} - {invoice.Number} ha sido registrado!"
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest(new {Ok = false, Msg = e.Message});
            }
        }

        /// <summary>
        /// registrar comprobante de compra.
        /// </summary>
        /// <param name="model">Comprobante</param>
        /// <returns>IActionResult</returns>
        [HttpPost("CreatePurchase")]
        public async Task<IActionResult> CreatePurchase([FromBody] Comprobante model)
        {
            try
            {
                _comprobanteService.SetModel(model);
                var invoice = await _comprobanteService.SavePurchase();
                model.InvoiceId = invoice.Id;

                return Ok(new
                {
                    Ok = true, Data = model,
                    Msg = $"{invoice.Serie} - {invoice.Number} ha sido registrado!"
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest(new {Ok = false, Msg = e.Message});
            }
        }
    }
}
