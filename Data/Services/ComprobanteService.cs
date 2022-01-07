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
    /// <summary>
    /// Registra los comprobantes en la Base de Datos.
    /// </summary>
    public class ComprobanteService : IComprobanteService
    {
        private readonly ILogger _logger;
        private readonly ApplicationDbContext _context;
        private Configuration _configuration;
        private Contact _contact;
        private Comprobante _comprobante;
        private Venta _venta;
        private NotaComprobante _notaComprobante;

        /// <summary>
        /// Constructor del Servicio.
        /// </summary>
        public ComprobanteService(ILogger<ComprobanteService> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        /// <summary>
        /// Establecer modelo comprobante.
        /// </summary>
        public void SetModel(Comprobante model)
        {
            _comprobante = model;
            _logger.LogInformation($"Comprobante: {JsonSerializer.Serialize(_comprobante)}");
        }

        /// <summary>
        /// Establecer modelo venta.
        /// </summary>
        public void SetModel(Venta model)
        {
            _venta = model;
            _logger.LogInformation($"Venta: {JsonSerializer.Serialize(_venta)}");
        }

        /// <summary>
        /// Guardar el Comprobante de venta.
        /// </summary>
        public async Task<Invoice> CreateSale(int serie)
        {
            await GetConfiguration();
            await GetContact(_comprobante.ContactId);
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var invoiceSerie = await GetInvoiceSerie(serie);
                var invoice = _comprobante.GetInvoice(_configuration, _contact);
                GenerateInvoiceSerie(ref invoiceSerie, ref invoice, _comprobante.DocType);

                // actualizar serie de facturación.
                _context.InvoiceSeries.Update(invoiceSerie);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Series de facturación actualizado!");

                // Agregar Información del comprobante.
                _context.Invoices.Add(invoice);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Información del comprobante agregado!");

                // Agregar detalles del comprobante.
                var invoiceDetails = _comprobante.GetInvoiceDetail(invoice.Id);
                _context.InvoiceDetails.AddRange(invoiceDetails);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Detalle del comprobante agregado!");

                // Agregar Tributos de Factura.
                var tributos = _comprobante.GetTributo(invoice.Id);
                _context.Tributos.AddRange(tributos);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Tributos de factura agregado!");

                // Agregar cuentas por cobrar si la factura es a crédito.
                if (_comprobante.FormaPago.Equals("Credito"))
                {
                    var invoiceAccounts = _comprobante.GetInvoiceAccounts(invoice);
                    _context.InvoiceAccounts.AddRange(invoiceAccounts);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Cuentas por cobrar agregado!");
                }

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
        /// Guardar comprobante de venta rápida.
        /// </summary>
        public async Task<Invoice> CreateQuickSale(int caja)
        {
            await GetConfiguration();
            await GetContact(_venta.ContactId);
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var cajaDiaria = await GetCajaDiaria(caja);
                var invoiceSerie = await GetInvoiceSerie(Convert.ToInt32(cajaDiaria.InvoiceSerieId));
                var invoice = _venta.GetInvoice(_configuration, _contact);
                GenerateInvoiceSerie(ref invoiceSerie, ref invoice, _venta.DocType);

                // actualizar serie de facturación.
                _context.InvoiceSeries.Update(invoiceSerie);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Series de facturación actualizado!");

                // Agregar Información del comprobante.
                _context.Invoices.Add(invoice);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Información del comprobante agregado!");

                // Agregar detalles del comprobante.
                var invoiceDetails = _venta.GetInvoiceDetail(invoice.Id);
                _context.InvoiceDetails.AddRange(invoiceDetails);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Detalle del comprobante agregado!");

                // Agregar Tributos de Factura.
                var tributos = _venta.GetTributo(invoice.Id);
                _context.Tributos.AddRange(tributos);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Tributos de factura agregado!");

                // Registrar operación de Caja.
                var cashierDetail = GetCashierDetail(invoice, _venta, cajaDiaria.Id);
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
        /// Guardar el Comprobante de compra.
        /// </summary>
        public async Task<Invoice> CreatePurchase()
        {
            await GetConfiguration();
            await GetContact(_comprobante.ContactId);
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Agregar Información del comprobante.
                var invoice = _comprobante.GetInvoice(_configuration, _contact);
                _context.Invoices.Add(invoice);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Información del comprobante agregado!");

                // Agregar detalles del comprobante.
                var invoiceDetails = _comprobante.GetInvoiceDetail(invoice.Id);
                _context.InvoiceDetails.AddRange(invoiceDetails);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Detalle del comprobante agregado!");

                // Agregar Tributos de Factura.
                var tributos = _comprobante.GetTributo(invoice.Id);
                _context.Tributos.AddRange(tributos);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Tributos de factura agregado!");

                // Agregar cuentas por pagar si la factura es a crédito.
                if (_comprobante.FormaPago.Equals("Credito"))
                {
                    var invoiceAccounts = _comprobante.GetInvoiceAccounts(invoice);
                    _context.InvoiceAccounts.AddRange(invoiceAccounts);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Cuentas por pagar agregado!");
                }

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
        /// Editar comprobante de compra.
        /// </summary>
        public async Task<Invoice> UpdatePurchase()
        {
            await GetConfiguration();
            await GetContact(_comprobante.ContactId);
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                if (_comprobante.InvoiceId == null)
                    throw new Exception("No existe ID del comprobante");
                if (!_comprobante.InvoiceType.Equals("COMPRA"))
                    throw new Exception("No es un comprobante de compra!");

                // Editar Información del comprobante.
                var invoice = await _context.Invoices.FindAsync(_comprobante.InvoiceId);
                UpdateInvoiceData(ref invoice);
                _context.Invoices.Update(invoice);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Información del comprobante actualizado!");

                // Editar detalles del comprobante.
                var invoiceDetails = await _context.InvoiceDetails.AsNoTracking()
                    .Where(m => m.InvoiceId.Equals(invoice.Id)).ToListAsync();
                _context.InvoiceDetails.RemoveRange(invoiceDetails);
                await _context.SaveChangesAsync();
                _context.InvoiceDetails.AddRange(_comprobante.GetInvoiceDetail(invoice.Id));
                await _context.SaveChangesAsync();
                _logger.LogInformation("Detalle del comprobante actualizado!");

                // Editar Tributos de Factura.
                var tributos = await _context.Tributos.AsNoTracking()
                    .Where(m => m.InvoiceId.Equals(invoice.Id)).ToListAsync();
                _context.Tributos.RemoveRange(tributos);
                await _context.SaveChangesAsync();
                _context.Tributos.AddRange(_comprobante.GetTributo(invoice.Id));
                await _context.SaveChangesAsync();
                _logger.LogInformation("Tributos de factura actualizado!");

                // editar cuentas por pagar si la factura es a crédito.
                if (_comprobante.FormaPago.Equals("Credito"))
                {
                    var nuevosRegistros = _comprobante.GetInvoiceAccounts(invoice);
                    var registrosAntiguos = await _context.InvoiceAccounts.AsNoTracking()
                        .Where(m => m.InvoiceId.Equals(invoice.Id)).ToListAsync();

                    var registrosParaBorrar = new List<InvoiceAccount>();
                    var registrosEditados = new List<InvoiceAccount>();
                    registrosAntiguos.ForEach(item =>
                    {
                        if (!nuevosRegistros.Exists(x => x.Id.Equals(item.Id)))
                            registrosParaBorrar.Add(item);
                        if (nuevosRegistros.Exists(x => x.Id.Equals(item.Id)))
                        {
                            var objTmp = nuevosRegistros.Find(x => x.Id.Equals(item.Id));
                            if (objTmp != null)
                            {
                                item.Cuota = objTmp.Cuota;
                                item.Amount = objTmp.Amount;
                                item.EndDate = objTmp.EndDate;
                                item.Year = objTmp.Year;
                                item.Month = objTmp.Month;
                                registrosEditados.Add(item);
                                nuevosRegistros.Remove(objTmp);
                            }
                        }
                    });

                    // actualizar cuotas de factura en la base de datos.
                    _context.InvoiceAccounts.RemoveRange(registrosParaBorrar);
                    await _context.SaveChangesAsync();
                    _context.InvoiceAccounts.UpdateRange(registrosEditados);
                    await _context.SaveChangesAsync();
                    _context.InvoiceAccounts.AddRange(nuevosRegistros);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Cuentas por pagar actualizado!");
                }

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
        /// Carga la configuración del sistema.
        /// </summary>
        private async Task GetConfiguration()
        {
            _configuration = await _context.Configuration.AsNoTracking().FirstAsync();
            _logger.LogInformation($"Configuración: {JsonSerializer.Serialize(_configuration)}");
        }

        /// <summary>
        /// Carga los datos de contacto.
        /// </summary>
        private async Task GetContact(int id)
        {
            _contact = await _context.Contacts.AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id.Equals(id));
            _logger.LogInformation($"Contacto: {JsonSerializer.Serialize(_contact)}");
        }

        /// <summary>
        /// Obtener serie de facturación.
        /// </summary>
        private async Task<InvoiceSerie> GetInvoiceSerie(int id)
        {
            var invoiceSerie = await _context.InvoiceSeries.FindAsync(id);
            if (invoiceSerie == null) throw new Exception("Serie comprobante no existe!");
            _logger.LogInformation($"Serie Comprobante: {JsonSerializer.Serialize(invoiceSerie)}");
            return invoiceSerie;
        }

        /// <summary>
        /// Obtener caja diaria.
        /// </summary>
        private async Task<CajaDiaria> GetCajaDiaria(int id)
        {
            var cajaDiaria = await _context.CajasDiaria.AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id.Equals(id));
            _logger.LogInformation($"Caja Diaria: {JsonSerializer.Serialize(cajaDiaria)}");
            return cajaDiaria;
        }

        /// <summary>
        /// Generar serie y número del comprobante de venta.
        /// </summary>
        private void GenerateInvoiceSerie(ref InvoiceSerie invoiceSerie, ref Invoice invoice, string docType)
        {
            int numComprobante = 0;
            string THROW_MESSAGE = "Ingresa serie de comprobante!";
            switch (docType)
            {
                case "FACTURA":
                    invoice.Serie = invoiceSerie.Factura;
                    numComprobante = Convert.ToInt32(invoiceSerie.CounterFactura);
                    if (numComprobante > 99999999)
                        throw new Exception(THROW_MESSAGE);
                    numComprobante = numComprobante + 1;
                    invoiceSerie.CounterFactura = numComprobante;
                    break;
                case "BOLETA":
                    invoice.Serie = invoiceSerie.Boleta;
                    numComprobante = Convert.ToInt32(invoiceSerie.CounterBoleta);
                    if (numComprobante > 99999999)
                        throw new Exception(THROW_MESSAGE);
                    numComprobante = numComprobante + 1;
                    invoiceSerie.CounterBoleta = numComprobante;
                    break;
                case "NOTA":
                    invoice.Serie = invoiceSerie.NotaDeVenta;
                    numComprobante = Convert.ToInt32(invoiceSerie.CounterNotaDeVenta);
                    if (numComprobante > 99999999)
                        throw new Exception(THROW_MESSAGE);
                    numComprobante = numComprobante + 1;
                    invoiceSerie.CounterNotaDeVenta = numComprobante;
                    break;
            }

            invoice.Number = numComprobante.ToString("D8");
        }

        /// <summary>
        /// Configurar Detalle de Caja Diaria.
        /// </summary>
        private CashierDetail GetCashierDetail(Invoice invoice, Venta model, int cajaDiaria)
        {
            return new CashierDetail()
            {
                CajaDiariaId = cajaDiaria,
                InvoiceId = invoice.Id,
                TypeOperation = TypeOperation.Comprobante,
                StartDate = DateTime.Now,
                Document = $"{invoice.Serie}-{invoice.Number}",
                Contact = invoice.RznSocialUsuario,
                Glosa = model.Remark,
                PaymentMethod = model.PaymentMethod,
                Type = "ENTRADA",
                Total = invoice.SumImpVenta
            };
        }

        /// <summary>
        /// Actualizar Información del Comprobante.
        /// </summary>
        private void UpdateInvoiceData(ref Invoice invoice)
        {
            var result = _comprobante.GetInvoice(_configuration, _contact);
            invoice.DocType = result.DocType;
            invoice.Serie = result.Serie;
            invoice.Number = result.Number;
            invoice.TipOperacion = result.TipOperacion;
            invoice.FecEmision = result.FecEmision;
            invoice.FecVencimiento = result.FecVencimiento;
            invoice.FormaPago = result.FormaPago;
            invoice.ContactId = result.ContactId;
            invoice.TipDocUsuario = result.TipDocUsuario;
            invoice.NumDocUsuario = result.NumDocUsuario;
            invoice.RznSocialUsuario = result.RznSocialUsuario;
            invoice.SumTotTributos = result.SumTotTributos;
            invoice.SumTotValVenta = result.SumTotValVenta;
            invoice.SumPrecioVenta = result.SumPrecioVenta;
            invoice.SumImpVenta = result.SumImpVenta;
            invoice.Year = result.Year;
            invoice.Month = result.Month;
        }

        /// <summary>
        /// Establecer modelo Nota comprobante.
        /// </summary>
        public void SetModel(NotaComprobante model)
        {
            _notaComprobante = model;
            _logger.LogInformation($"Nota Comprobante: {JsonSerializer.Serialize(_notaComprobante)}");
        }

        /// <summary>
        /// Registrar Nota de Crédito/Débito.
        /// </summary>
        public async Task<InvoiceNote> CreateNote()
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var invoice = await _context.Invoices.AsNoTracking()
                    .FirstOrDefaultAsync(m => m.Id.Equals(_notaComprobante.InvoiceId));
                if (invoice == null) throw new Exception("No existe comprobante!");

                string tipoNota = string.Empty;
                if (_notaComprobante.DocType.Equals("NC")) tipoNota = "crédito";
                if (_notaComprobante.DocType.Equals("ND")) tipoNota = "débito";

                // Agregar Información de la Nota crédito/débito.
                var invoiceNote = _notaComprobante.GetInvoiceNote(invoice);
                _context.InvoiceNotes.Add(invoiceNote);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Información de la Nota {tipoNota} agregado!");

                // Agregar detalles de la Nota crédito/débito.
                var invoiceNoteDetails = _notaComprobante.GetInvoiceNoteDetail(invoiceNote.Id);
                _context.InvoiceNoteDetails.AddRange(invoiceNoteDetails);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Detalle de la Nota {tipoNota} agregado!");

                // confirmar transacción.
                await transaction.CommitAsync();
                _logger.LogInformation("Transacción confirmada!");
                return invoiceNote;
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                _logger.LogInformation("La transacción ha sido cancelada!");
                _logger.LogError(e.Message);
            }

            throw new Exception("Hubo un error en la emisión de la Nota!");
        }
    }
}
