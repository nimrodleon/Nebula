using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using CpeLibPE.Facturador;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
            bool result = false;
            await GetConfiguration();
            var invoice = await _context.Invoices.AsNoTracking()
                .Include(m => m.InvoiceDetails)
                .Include(m => m.Tributos)
                .SingleAsync(m => m.Id.Equals(id));
            var cabecera = new sfs.Invoice()
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
                sumImpVenta = Convert.ToDecimal(invoice.SumImpVenta).ToString("N2"),
            };
            var detalle = new List<sfs.InvoiceDetail>();
            invoice.InvoiceDetails.ForEach(item =>
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
                    tipAfeIGV = item.TipAfeIgv,
                    porIgvItem = item.PorIgvItem,
                    // Importe de Venta.
                    mtoPrecioVentaUnitario = Convert.ToDecimal(item.MtoPrecioVentaUnitario).ToString("N2"),
                    mtoValorVentaItem = Convert.ToDecimal(item.MtoValorVentaItem).ToString("N2")
                });
            });

            var json = new JsonBoletaParser()
            {
                cabecera = cabecera,
                detalle = detalle
            };

            return result;
        }
    }
}
