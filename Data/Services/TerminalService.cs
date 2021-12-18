using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nebula.Data.Models;
using Nebula.Data.ViewModels;

namespace Nebula.Data.Services
{
    public class TerminalService : ITerminalService
    {
        private readonly ILogger _logger;
        private readonly ApplicationDbContext _context;
        private Configuration _configuration;
        private CajaDiaria _cajaDiaria;
        private Contact _client;
        private Sale _model;

        /// <summary>
        /// Constructor del Servicio.
        /// </summary>
        public TerminalService(ILogger<TerminalService> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        /// <summary>
        /// Establecer modelo.
        /// </summary>
        public void SetModel(Sale model)
        {
            _model = model;
            _logger.LogInformation($"Venta: {JsonSerializer.Serialize(model)}");
        }

        /// <summary>
        /// Guardar Factura en la Base de datos.
        /// </summary>
        public async Task<Invoice> SaveInvoice(int cajaDiaria)
        {
            await GetConfiguration();
            await GetClientData(_model.ClientId);
            await GetCajaDiaria(cajaDiaria);

            // configurar transacción de la base de datos.
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var invoice = GetInvoice();
                var invoiceSerie = await GetInvoiceSerie(_cajaDiaria.InvoiceSerieId);

                int numComprobante = 0;
                string THROW_MESSAGE = "Ingresa serie de comprobante!";
                switch (_model.DocType)
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
                _logger.LogInformation("Contador de Series de facturación actualizado!");

                // registrar comprobante.
                _context.Invoices.Add(invoice);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Cabecera comprobante registrado!");

                // Agregar detalle del comprobante.
                var invoiceDetails = GetInvoiceDetails(invoice.Id);
                _context.InvoiceDetails.AddRange(invoiceDetails);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Detalle comprobante registrado!");

                // Registrar operación de Caja.
                var cashierDetail = GetCashierDetail(invoice);
                _context.CashierDetails.Add(cashierDetail);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Operación de caja registrada!");

                // confirmar transacción.
                await transaction.CommitAsync();
                _logger.LogInformation("Transacción confirmada!");
                return invoice;
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                _logger.LogInformation("La transacción ha sido cancelada!");
                _logger.LogError(e.Message);
            }

            throw new Exception("Hubo un error en la emisión del comprobante!");
        }

        /// <summary>
        /// Configurar cabecera de venta.
        /// </summary>
        private Invoice GetInvoice()
        {
            return new Invoice()
            {
                DocType = _model.DocType,
                TipOperacion = "0101",
                FecEmision = DateTime.Now.ToString("yyyy-MM-dd"),
                HorEmision = DateTime.Now.ToString("HH:mm:ss"),
                FormaPago = "Contado",
                TipDocUsuario = _client.DocType.ToString(),
                NumDocUsuario = _client.Document,
                RznSocialUsuario = _client.Name,
                TipMoneda = _configuration.TipMoneda,
                SumTotValVenta = _model.SumTotValVenta, // refactoring
                SumTotTributos = _model.SumTotTributos, // refactoring
                SumImpVenta = _model.SumImpVenta, // refactoring
                InvoiceType = "SALE",
                Year = DateTime.Now.ToString("yyyy"),
                Month = DateTime.Now.ToString("MM"),
            };
        }

        /// <summary>
        /// Configurar detalle de comprobante.
        /// </summary>
        private List<InvoiceDetail> GetInvoiceDetails(int? invoice)
        {
            var invoiceDetails = new List<InvoiceDetail>();
            _model.Details.ForEach(item =>
            {
                var product = GetProduct(item.ProductId);
                if (product != null)
                {
                    // Tributo: Afectación al IGV por ítem.
                    string tipAfeIgv = "10";
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

                    // calculo base imponible.
                    var valorIgv = ((_configuration.PorcentajeIgv / 100) + 1);
                    var precioVenta = item.Price * item.Quantity;
                    var baseImponible = precioVenta / valorIgv;

                    // agregar items al comprobante.
                    invoiceDetails.Add(new InvoiceDetail()
                    {
                        InvoiceId = invoice,
                        CodUnidadMedida = product.UndMedida.SunatCode,
                        CtdUnidadItem = item.Quantity,
                        CodProducto = product.Id.ToString(),
                        CodProductoSunat = product.Barcode.Length == 0 ? "-" : product.Barcode,
                        DesItem = product.Description,
                        MtoValorUnitario = baseImponible,
                        SumTotTributosItem = precioVenta - baseImponible,
                        CodTriIgv = "1000",
                        MtoIgvItem = precioVenta - baseImponible,
                        MtoBaseIgvItem = baseImponible,
                        NomTributoIgvItem = "IGV",
                        CodTipTributoIgvItem = "VAT",
                        TipAfeIgv = tipAfeIgv,
                        PorIgvItem = Convert.ToDecimal(_configuration.PorcentajeIgv).ToString("N2"),
                        CodTriIcbper = "7152",
                        MtoTriIcbperItem = 0,
                        CtdBolsasTriIcbperItem = 0,
                        NomTributoIcbperItem = "ICBPER",
                        CodTipTributoIcbperItem = "OTH",
                        MtoTriIcbperUnidad = _configuration.ValorImpuestoBolsa,
                        MtoPrecioVentaUnitario = precioVenta,
                        MtoValorVentaItem = baseImponible,
                    });
                }
            });
            return invoiceDetails;
        }

        /// <summary>
        /// Configurar Detalle de Caja Diaria.
        /// </summary>
        private CashierDetail GetCashierDetail(Invoice invoice)
        {
            return new CashierDetail()
            {
                CajaDiariaId = _cajaDiaria.Id,
                InvoiceId = invoice.Id,
                TypeOperation = TypeOperation.Comprobante,
                StartDate = DateTime.Now,
                Document = $"{invoice.Serie}-{invoice.Number}",
                Contact = invoice.RznSocialUsuario,
                Glosa = _model.Remark,
                PaymentMethod = _model.PaymentMethod,
                Type = "ENTRADA",
                Total = invoice.SumImpVenta
            };
        }

        /// <summary>
        /// Cargar configuración del Sistema.
        /// </summary>
        private async Task GetConfiguration()
        {
            _configuration = await _context.Configuration.AsNoTracking().FirstAsync();
            _logger.LogInformation($"Configuración: {JsonSerializer.Serialize(_configuration)}");
        }

        /// <summary>
        /// Cargar datos del cliente.
        /// </summary>
        private async Task GetClientData(int id)
        {
            _client = await _context.Contacts.AsNoTracking()
                .SingleAsync(m => m.Id.Equals(id));
            _logger.LogInformation($"Cliente: {JsonSerializer.Serialize(_client)}");
        }

        /// <summary>
        /// Cargar caja diaria.
        /// </summary>
        private async Task GetCajaDiaria(int id)
        {
            _cajaDiaria = await _context.CajasDiaria.AsNoTracking()
                .SingleAsync(m => m.Id.Equals(id));
            _logger.LogInformation($"Caja Diaria: {JsonSerializer.Serialize(_cajaDiaria)}");
        }

        /// <summary>
        /// Cargar series de facturación.
        /// </summary>
        private async Task<InvoiceSerie> GetInvoiceSerie(Guid? id)
        {
            var invoiceSerie = await _context.InvoiceSeries.SingleAsync(m => m.Id.Equals(id));
            if (invoiceSerie == null) throw new Exception("Serie comprobante no existe!");
            _logger.LogInformation($"Serie Comprobante: {JsonSerializer.Serialize(invoiceSerie)}");
            return invoiceSerie;
        }

        /// <summary>
        /// Obtener producto por id.
        /// </summary>
        private Product GetProduct(int id)
        {
            var product = _context.Products.AsNoTracking()
                .Include(m => m.UndMedida)
                .Single(m => m.Id.Equals(id));
            _logger.LogInformation($"Producto: {JsonSerializer.Serialize(product)}");
            return product;
        }
    }
}
