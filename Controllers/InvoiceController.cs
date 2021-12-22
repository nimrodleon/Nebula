using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nebula.Data;
using Nebula.Data.Models;
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
        private readonly ITerminalService _terminalService;
        private readonly ICpeService _cpeService;

        public InvoiceController(ILogger<InvoiceController> logger,
            ApplicationDbContext context, ITerminalService terminalService, ICpeService cpeService)
        {
            _logger = logger;
            _context = context;
            _terminalService = terminalService;
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
                .FirstOrDefaultAsync(m => m.Id.Equals(id));
            if (result == null) return BadRequest();
            return Ok(result);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] Comprobante model)
        {
            _logger.LogInformation(JsonSerializer.Serialize(model));
            string invoiceType = model.InvoiceType.ToUpper();

            // información del cliente.
            var client = await _context.Contacts.AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id.Equals(model.ContactId));

            // información de venta.
            var invoice = new Invoice()
            {
                DocType = model.DocType,
                TipOperacion = model.TypeOperation,
                FecEmision = invoiceType.Equals("SALE")
                    ? DateTime.Now.ToString("yyyy-MM-dd")
                    : model.StartDate.ToString("yyyy-MM-dd"),
                HorEmision = invoiceType.Equals("SALE") ? DateTime.Now.ToString("HH:mm:ss") : "-",
                FecVencimiento = model.PaymentType.Equals("Credito") ? model.EndDate.ToString("yyyy-MM-dd") : "-",
                FormaPago = model.PaymentType,
                TipDocUsuario = client.DocType.ToString(),
                NumDocUsuario = client.Document,
                RznSocialUsuario = client.Name,
                TipMoneda = "PEN",
                SumTotTributos = model.SumTotTributos,
                SumTotValVenta = model.SumTotValVenta,
                SumImpVenta = model.SumImpVenta,
                InvoiceType = model.InvoiceType.ToUpper(),
                Year = invoiceType.Equals("SALE") ? DateTime.Now.ToString("yyyy") : model.StartDate.ToString("yyyy"),
                Month = invoiceType.Equals("SALE") ? DateTime.Now.ToString("MM") : model.StartDate.ToString("MM"),
            };

            var cajaDiaria = await _context.CajasDiaria.AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id.Equals(model.CajaDiariaId));

            // guardar en la base de datos.
            await using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // comprobante - compra/venta.
                    switch (model.InvoiceType.ToUpper())
                    {
                        case "SALE":
                            var invoiceSerie = await _context.InvoiceSeries.FirstOrDefaultAsync(m =>
                                m.Id.Equals(cajaDiaria.InvoiceSerieId));
                            if (invoiceSerie == null) throw new Exception("No existe serie comprobante!");
                            int numComprobante = 0;
                            string THROW_MESSAGE = "Ingresa serie de comprobante!";
                            switch (model.DocType)
                            {
                                case "FT":
                                    invoice.Serie = invoiceSerie.Factura;
                                    numComprobante = invoiceSerie.CounterFactura;
                                    if (numComprobante > 99999999)
                                        throw new Exception(THROW_MESSAGE);
                                    numComprobante = numComprobante + 1;
                                    invoiceSerie.CounterFactura = numComprobante;
                                    break;
                                case "BL":
                                    invoice.Serie = invoiceSerie.Boleta;
                                    numComprobante = invoiceSerie.CounterBoleta;
                                    if (numComprobante > 99999999)
                                        throw new Exception(THROW_MESSAGE);
                                    numComprobante = numComprobante + 1;
                                    invoiceSerie.CounterBoleta = numComprobante;
                                    break;
                                case "NV":
                                    invoice.Serie = invoiceSerie.NotaDeVenta;
                                    numComprobante = invoiceSerie.CounterNotaDeVenta;
                                    if (numComprobante > 99999999)
                                        throw new Exception(THROW_MESSAGE);
                                    numComprobante = numComprobante + 1;
                                    invoiceSerie.CounterNotaDeVenta = numComprobante;
                                    break;
                            }

                            invoice.Number = numComprobante.ToString("D8");
                            _context.InvoiceSeries.Update(invoiceSerie);
                            await _context.SaveChangesAsync();
                            _logger.LogInformation("El contador de la serie ha sido actualizado!");
                            break;
                        case "PURCHASE":
                            invoice.Serie = model.Serie;
                            invoice.Number = model.Number;
                            break;
                    }

                    // registrar comprobante.
                    _context.Invoices.Add(invoice);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("La cabecera del comprobante ha sido registrado");

                    // agregar detalle del comprobante.
                    model.Details.ForEach(item =>
                    {
                        var product = _context.Products.AsNoTracking()
                            .Include(m => m.UndMedida)
                            .FirstOrDefault(m => m.Id.Equals(item.ProductId));

                        if (product != null)
                        {
                            // Tributo: Afectación al IGV por ítem.
                            var tipAfeIgv = "10";
                            switch (product.IgvSunat)
                            {
                                case "GRAVADO":
                                    tipAfeIgv = "10";
                                    break;
                                case "EXONERADO":
                                    tipAfeIgv = "20";
                                    break;
                                case "INAFECTO":
                                    tipAfeIgv = "30";
                                    break;
                            }

                            // agregar item al detalle del comprobante.
                            _context.InvoiceDetails.Add(new InvoiceDetail()
                            {
                                Invoice = invoice,
                                CodUnidadMedida = product.UndMedida.SunatCode,
                                CtdUnidadItem = item.Quantity,
                                CodProducto = item.ProductId.ToString(),
                                CodProductoSunat = product.Barcode,
                                DesItem = product.Description,
                                MtoValorUnitario = item.Price,
                                SumTotTributosItem = (item.Price * item.Quantity) * 0.18M,
                                CodTriIgv = "1000",
                                MtoIgvItem = 18.0M,
                                MtoBaseIgvItem = item.Price * item.Quantity,
                                NomTributoIgvItem = "IGV",
                                CodTipTributoIgvItem = "VAT",
                                TipAfeIgv = tipAfeIgv,
                                PorIgvItem = 18.ToString("N2"),
                                CodTriIcbper = "7152",
                                MtoTriIcbperItem = 0,
                                CtdBolsasTriIcbperItem = 0,
                                NomTributoIcbperItem = "ICBPER",
                                CodTipTributoIcbperItem = "OTH",
                                MtoTriIcbperUnidad = 0.3M,
                                MtoPrecioVentaUnitario = item.Price * item.Quantity,
                                MtoValorVentaItem = (item.Price * item.Quantity) * 1.18M,
                            });
                            _context.SaveChanges();
                            _logger.LogInformation("Item comprobante agregado!");
                        }
                    });

                    // confirmar transacción.
                    await transaction.CommitAsync();
                    _logger.LogInformation("Transacción confirmada!");

                    return Ok(new
                    {
                        Ok = true, Data = model,
                        Msg = $"{invoice.Serie} - {invoice.Number} ha sido registrado!"
                    });
                }
                catch (Exception e)
                {
                    await transaction.RollbackAsync();
                    _logger.LogInformation("La transacción ha sido cancelada!");
                    _logger.LogError(e.Message);
                }
            }

            return BadRequest(new
            {
                Ok = false, Msg = "Hubo un error en la emisión del comprobante!"
            });
        }

        [HttpPost("SalePos/{id}")]
        public async Task<IActionResult> SalePos(int id, [FromBody] Sale model)
        {
            try
            {
                _terminalService.SetModel(model);
                var invoice = await _terminalService.SaveInvoice(id);
                bool fileExist = false;
                fileExist = invoice.DocType.Equals("BL") && await _cpeService.CreateBoletaJson(invoice.Id);
                // factura...


                model.Vuelto = model.MontoTotal - model.SumImpVenta;
                return Ok(new
                {
                    Ok = fileExist, Data = model, Msg = $"{invoice.Serie} - {invoice.Number} ha sido registrado!"
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
