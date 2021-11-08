using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nebula.Data;
using Nebula.Data.Models;
using Nebula.Data.ViewModels;

namespace Nebula.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public InvoiceController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("Store")]
        public async Task<IActionResult> Store([FromBody] Invoice model)
        {
            _context.Invoices.Add(model);
            await _context.SaveChangesAsync();
            return Ok(new
            {
                Ok = true, Data = model,
                Msg = $" ha sido registrado!"
            });
        }

        [HttpPost("SalePos")]
        public async Task<IActionResult> SalePos([FromBody] Sale model)
        {
            var client = await _context.Contacts.AsNoTracking()
                .Include(m => m.PeopleDocType)
                .FirstOrDefaultAsync(m => m.Id.Equals(model.ClientId));
            // detalle venta.
            var details = new List<InvoiceDetail>();
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

                    details.Add(new InvoiceDetail()
                    {
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
                }
            });
            // información venta.
            var invoice = new Invoice()
            {
                TypeDoc = model.DocType,
                TipOperacion = "0101",
                FecEmision = DateTime.Now.ToString("yyyy-MM-dd"),
                HorEmision = DateTime.Now.ToString("HH:mm:ss"),
                FecVencimiento = model.EndDate.ToString("yyyy-MM-dd"),
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
                InvoiceDetails = details
            };

            // guardar en la base de datos.
            await using (var transaction = await _context.Database.BeginTransactionAsync())
            {
            }

            return Ok();
        }
    }
}
