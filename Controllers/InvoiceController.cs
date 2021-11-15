using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nebula.Data;
using Nebula.Data.Models;
using Nebula.Data.ViewModels;

namespace Nebula.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly ApplicationDbContext _context;

        public InvoiceController(ILogger<InvoiceController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
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
        public async Task<IActionResult> Show(int? id)
        {
            if (id == null) return BadRequest();
            var result = await _context.Invoices.AsNoTracking()
                .Include(m => m.InvoiceDetails)
                .FirstOrDefaultAsync(m => m.Id.Equals(id));
            if (result == null) return BadRequest();
            return Ok(result);
        }

        /// <summary>
        /// emisión comprobantes de pago,
        /// desde el módulo invoice de angular.
        /// </summary>
        [HttpPost("Store")]
        public async Task<IActionResult> Store([FromBody] Comprobante model)
        {
            _logger.LogInformation(JsonSerializer.Serialize(model));
            string invoiceType = model.InvoiceType.ToUpper();

            // información del cliente.
            var client = await _context.Contacts.AsNoTracking()
                .Include(m => m.PeopleDocType)
                .FirstOrDefaultAsync(m => m.Id.Equals(model.ContactId));

            // información de venta.
            var invoice = new Invoice()
            {
                TypeDoc = model.DocType,
                TipOperacion = model.TypeOperation,
                FecEmision = invoiceType.Equals("SALE")
                    ? DateTime.Now.ToString("yyyy-MM-dd")
                    : model.StartDate.ToString("yyyy-MM-dd"),
                HorEmision = invoiceType.Equals("SALE") ? DateTime.Now.ToString("HH:mm:ss") : "-",
                FecVencimiento = model.PaymentType.Equals("Credito") ? model.EndDate.ToString("yyyy-MM-dd") : "-",
                CodLocalEmisor = "0000",
                TipDocUsuario = client.PeopleDocType.SunatCode,
                NumDocUsuario = client.Document,
                RznSocialUsuario = client.Name,
                TipMoneda = "PEN",
                SumTotTributos = model.SumTotTributos,
                SumTotValVenta = model.SumTotValVenta,
                SumPrecioVenta = model.SumImpVenta,
                SumDescTotal = 0,
                SumOtrosCargos = 0,
                SumTotalAnticipos = 0,
                SumImpVenta = model.SumImpVenta,
                InvoiceType = model.InvoiceType.ToUpper(),
            };

            // guardar en la base de datos.
            await using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // comprobante - compra/venta.
                    switch (model.InvoiceType.ToUpper())
                    {
                        case "SALE":
                            var serieInvoice = await _context.SerieInvoices.FirstOrDefaultAsync(m =>
                                m.CajaId.ToString().Equals(model.CajaId) && m.DocType.Equals(model.DocType));
                            if (serieInvoice == null) throw new Exception("No existe serie comprobante!");
                            int numComprobante = Convert.ToInt32(serieInvoice.Counter);
                            if (numComprobante > 99999999)
                                throw new Exception("Ingresa nueva serie de comprobante!");
                            numComprobante = numComprobante + 1;
                            serieInvoice.Counter = numComprobante;
                            _context.SerieInvoices.Update(serieInvoice);
                            await _context.SaveChangesAsync();
                            _logger.LogInformation("El contador de la serie ha sido actualizado!");
                            invoice.Serie = serieInvoice.Prefix;
                            invoice.Number = numComprobante.ToString("D8");
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
                                MtoValorReferencialUnitario = 0,
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

        /// <summary>
        /// emisión comprobantes de pago,
        /// desde la terminal de venta.
        /// </summary>
        [HttpPost("SalePos/{id}")]
        public async Task<IActionResult> SalePos(int? id, [FromBody] Sale model)
        {
            _logger.LogInformation(JsonSerializer.Serialize(model));
            // información de serie factura.
            if (id == null)
                return BadRequest(new {Ok = false, Msg = "Debe enviar un ID valido!"});
            _logger.LogInformation("ID valido!");
            var cajaDiaria = await _context.CajasDiaria.AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id.Equals(id));
            if (cajaDiaria == null)
                return BadRequest(new {Ok = false, Msg = "No existe caja diaria!"});
            _logger.LogInformation($"Caja diaria [{cajaDiaria.Id}/{cajaDiaria.Name}]");
            var serieInvoice = await _context.SerieInvoices.FirstOrDefaultAsync(m =>
                m.CajaId.Equals(cajaDiaria.CajaId) && m.DocType.Equals(model.DocType));
            if (serieInvoice == null) return BadRequest(new {Ok = false, Msg = "Serie comprobante no existe!"});
            _logger.LogInformation($"Serie comprobante [{serieInvoice.Prefix}-{serieInvoice.Counter}]");
            // información del cliente.
            var client = await _context.Contacts.AsNoTracking()
                .Include(m => m.PeopleDocType)
                .FirstOrDefaultAsync(m => m.Id.Equals(model.ClientId));
            _logger.LogInformation($"Cliente - {client.Name}");
            // información venta.
            var invoice = new Invoice()
            {
                TypeDoc = model.DocType,
                TipOperacion = "0101",
                FecEmision = DateTime.Now.ToString("yyyy-MM-dd"),
                HorEmision = DateTime.Now.ToString("HH:mm:ss"),
                FecVencimiento = model.PaymentType.Equals("Credito") ? model.EndDate.ToString("yyyy-MM-dd") : "-",
                CodLocalEmisor = "0000",
                TipDocUsuario = client.PeopleDocType.SunatCode,
                NumDocUsuario = client.Document,
                RznSocialUsuario = client.Name,
                TipMoneda = "PEN",
                SumTotTributos = model.SumTotTributos,
                SumTotValVenta = model.SumTotValVenta,
                SumPrecioVenta = model.SumImpVenta,
                SumDescTotal = 0,
                SumOtrosCargos = 0,
                SumTotalAnticipos = 0,
                SumImpVenta = model.SumImpVenta
            };

            // guardar en la base de datos.
            await using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    int numComprobante = Convert.ToInt32(serieInvoice.Counter);
                    if (numComprobante > 99999999)
                        return BadRequest(new {Ok = false, Msg = "Ingresa nueva serie de comprobante!"});
                    numComprobante = numComprobante + 1;
                    serieInvoice.Counter = numComprobante;
                    _context.SerieInvoices.Update(serieInvoice);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("El contador de la serie ha sido actualizado!");

                    // registrar factura/boleta.
                    invoice.Serie = serieInvoice.Prefix;
                    invoice.Number = numComprobante.ToString("D8");
                    _context.Invoices.Add(invoice);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("La cabecera del comprobante ha sido registrado");

                    // agregar detalle de factura.
                    model.Details.ForEach(item =>
                    {
                        // información del producto.
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

                            // agregar nuevo item factura al contexto.
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
                                MtoValorReferencialUnitario = 0,
                            });
                            _context.SaveChanges();
                            _logger.LogInformation("Item comprobante agregado!");
                        }
                    });

                    // registrar operación de caja.
                    var cashierDetail = new CashierDetail()
                    {
                        CajaDiariaId = cajaDiaria.Id,
                        InvoiceId = invoice.Id,
                        TypeOperation = TypeOperation.Comprobante,
                        StartDate = DateTime.Now,
                        Document = string.Format($"{invoice.Serie}-{invoice.Number}"),
                        Contact = invoice.RznSocialUsuario,
                        Glosa = model.Remark,
                        Type = "Ingreso",
                        Total = invoice.SumImpVenta
                    };
                    _context.CashierDetails.Add(cashierDetail);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Operación de caja registrada!");

                    // confirmar transacción.
                    await transaction.CommitAsync();
                    _logger.LogInformation("Transacción confirmada!");

                    model.Vuelto = model.MontoTotal - model.SumImpVenta;
                    return Ok(new
                    {
                        Ok = true, Data = model,
                        Msg = $"{invoice.Serie} - {invoice.Number} ha sido registrado!"
                    });
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    _logger.LogInformation("La transacción ha sido cancelada!");
                }
            }

            return BadRequest(new
            {
                Ok = false, Msg = $"Hubo un error en la emisión del comprobante!"
            });
        }
    }
}
