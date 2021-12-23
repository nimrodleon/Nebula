using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using CpeLibPE.Facturador;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nebula.Data.Helpers;
using Nebula.Data.Models;
using sfs = CpeLibPE.Facturador.Models;

namespace Nebula.Data.Services
{
    public class CpeService : ICpeService
    {
        private readonly ILogger _logger;
        private readonly ApplicationDbContext _context;
        private Configuration _configuration;

        public CpeService(ILogger<CpeService> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
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
        /// Crear Archivo Json Boleta.
        /// </summary>
        public async Task<bool> CreateBoletaJson(int id)
        {
            await GetConfiguration();
            var invoice = await _context.Invoices.AsNoTracking()
                .Include(m => m.InvoiceDetails)
                .Include(m => m.Tributos)
                .SingleAsync(m => m.Id.Equals(id));

            var cabecera = GetInvoice(invoice);
            var detalle = GetInvoiceDetail(invoice.InvoiceDetails);
            var tributos = GetTributos(invoice.Tributos);
            var leyendas = GetLeyendas(invoice);

            // Configurar estructura de la boleta.
            var boleta = new JsonBoletaParser()
            {
                cabecera = cabecera,
                detalle = detalle,
                tributos = tributos,
                leyendas = leyendas
            };

            // Escribir datos en el Disco duro.
            string fileName = Path.Combine("DATA", $"{_configuration.Ruc}-03-{invoice.Serie}-{invoice.Number}.json");
            boleta.CreateJson(Path.Combine(_configuration.FileSunat, fileName));
            return File.Exists(Path.Combine(_configuration.FileSunat, fileName));
        }

        /// <summary>
        /// Comprobar si Existe operaciones gratuitas.
        /// </summary>
        private bool ExistFreeOperations(List<Tributo> tributos)
        {
            bool result = false;
            tributos.ForEach(item => result = item.IdeTributo.Equals("9996"));
            return result;
        }

        /// <summary>
        /// Configurar cabecera del comprobante.
        /// </summary>
        private sfs.Invoice GetInvoice(Invoice invoice)
        {
            return new sfs.Invoice()
            {
                tipOperacion = invoice.TipOperacion,
                fecEmision = invoice.FecEmision,
                horEmision = invoice.HorEmision,
                codLocalEmisor = _configuration.CodLocalEmisor,
                tipDocUsuario = invoice.TipDocUsuario,
                numDocUsuario = invoice.NumDocUsuario,
                rznSocialUsuario = invoice.RznSocialUsuario,
                tipMoneda = invoice.TipMoneda,
                sumTotTributos = Convert.ToDecimal(invoice.SumTotTributos).ToString("N2"),
                sumTotValVenta = Convert.ToDecimal(invoice.SumTotValVenta).ToString("N2"),
                sumPrecioVenta = Convert.ToDecimal(invoice.SumImpVenta).ToString("N2"),
                sumImpVenta = Convert.ToDecimal(invoice.SumImpVenta).ToString("N2"),
            };
        }

        /// <summary>
        /// Configurar detalle del comprobante.
        /// </summary>
        private List<sfs.InvoiceDetail> GetInvoiceDetail(List<InvoiceDetail> details)
        {
            var detalle = new List<sfs.InvoiceDetail>();
            details.ForEach(item =>
            {
                detalle.Add(new sfs.InvoiceDetail()
                {
                    codUnidadMedida = item.CodUnidadMedida,
                    ctdUnidadItem = item.CtdUnidadItem.ToString(),
                    codProducto = item.CodProducto,
                    codProductoSUNAT = item.CodProductoSunat,
                    desItem = item.DesItem,
                    mtoValorUnitario = Convert.ToDecimal(item.MtoValorUnitario).ToString("N2"),
                    sumTotTributosItem = Convert.ToDecimal(item.SumTotTributosItem).ToString("N2"),
                    // Tributo: IGV(1000).
                    codTriIGV = item.CodTriIgv,
                    mtoIgvItem = Convert.ToDecimal(item.MtoIgvItem).ToString("N2"),
                    mtoBaseIgvItem = Convert.ToDecimal(item.MtoBaseIgvItem).ToString("N2"),
                    nomTributoIgvItem = item.NomTributoIgvItem,
                    codTipTributoIgvItem = item.CodTipTributoIgvItem,
                    tipAfeIGV = item.TipAfeIgv,
                    porIgvItem = item.PorIgvItem,
                    // Tributo ICBPER 7152.
                    codTriIcbper = item.CodTriIcbper,
                    mtoTriIcbperItem = Convert.ToDecimal(item.MtoTriIcbperItem).ToString("N2"),
                    ctdBolsasTriIcbperItem = item.CtdBolsasTriIcbperItem.ToString(),
                    nomTributoIcbperItem = item.NomTributoIcbperItem,
                    codTipTributoIcbperItem = item.CodTipTributoIcbperItem,
                    mtoTriIcbperUnidad = Convert.ToDecimal(item.MtoTriIcbperItem).ToString("N2"),
                    // Importe de Venta.
                    mtoPrecioVentaUnitario = Convert.ToDecimal(item.MtoPrecioVentaUnitario).ToString("N2"),
                    mtoValorVentaItem = Convert.ToDecimal(item.MtoValorVentaItem).ToString("N2")
                });
            });
            return detalle;
        }

        /// <summary>
        /// Configurar Tributos del Comprobante.
        /// </summary>
        private List<sfs.Tributo> GetTributos(List<Tributo> tributos)
        {
            var result = new List<sfs.Tributo>();
            tributos.ForEach(item =>
            {
                result.Add(new sfs.Tributo()
                {
                    ideTributo = item.IdeTributo,
                    nomTributo = item.NomTributo,
                    codTipTributo = item.CodTipTributo,
                    mtoBaseImponible = Convert.ToDecimal(item.MtoBaseImponible).ToString("N2"),
                    mtoTributo = Convert.ToDecimal(item.MtoTributo).ToString("N2")
                });
            });
            return result;
        }

        /// <summary>
        /// Configurar Leyendas del Comprobante.
        /// </summary>
        private List<sfs.Leyenda> GetLeyendas(Invoice invoice)
        {
            var leyendas = new List<sfs.Leyenda>();
            if (ExistFreeOperations(invoice.Tributos))
            {
                leyendas.Add(new sfs.Leyenda()
                {
                    codLeyenda = "1002",
                    desLeyenda = "TRANSFERENCIA GRATUITA DE UN BIEN Y/O SERVICIO PRESTADO GRATUITAMENTE"
                });
            }

            leyendas.Add(new sfs.Leyenda()
            {
                codLeyenda = "1000",
                desLeyenda = new NumberToLetters(Convert.ToDecimal(invoice.SumImpVenta)).ToString()
            });
            return leyendas;
        }
    }
}
