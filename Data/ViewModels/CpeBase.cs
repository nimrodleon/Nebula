using System;
using System.Collections.Generic;
using Nebula.Data.Models;

namespace Nebula.Data.ViewModels
{
    public class CpeBase
    {
        /// <summary>
        /// Id del Contacto.
        /// </summary>
        public int ContactId { get; set; }

        /// <summary>
        /// Tipo documento.
        /// </summary>
        public string DocType { get; set; }

        /// <summary>
        /// SubTotal.
        /// </summary>
        public decimal SumTotValVenta { get; set; }

        /// <summary>
        /// Sumatoria Tributos.
        /// </summary>
        public decimal SumTotTributos { get; set; }

        /// <summary>
        /// Importe a Cobrar.
        /// </summary>
        public decimal SumImpVenta { get; set; }

        /// <summary>
        /// Observación o Comentario.
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// Tipo comprobante #COMPRA/VENTA.
        /// </summary>
        public string InvoiceType { get; set; } = "VENTA";

        /// <summary>
        /// Detalle de Venta.
        /// </summary>
        public List<CpeDetail> Details { get; set; }

        /// <summary>
        /// Cuotas a Crédito.
        /// </summary>
        public List<Cuota> Cuotas { get; set; }

        /// <summary>
        /// Id del Comprobante.
        /// </summary>
        public int? InvoiceId { get; set; }

        /// <summary>
        /// calcular importe de venta.
        /// </summary>
        protected void CalcImporteVenta()
        {
            decimal icbper = 0;
            SumTotValVenta = 0;
            SumTotTributos = 0;
            Details.ForEach(item =>
            {
                SumTotValVenta = SumTotValVenta + item.MtoBaseIgvItem;
                SumTotTributos = SumTotTributos + item.MtoIgvItem;
                icbper = icbper + item.MtoTriIcbperItem;
            });
            SumImpVenta = SumTotValVenta + SumTotTributos + icbper;
        }

        /// <summary>
        /// Configurar Detalle de Facturas.
        /// </summary>
        public List<InvoiceDetail> GetInvoiceDetail(int invoice)
        {
            var invoiceDetails = new List<InvoiceDetail>();
            Details.ForEach(item =>
            {
                // Tributo: Afectación al IGV por ítem.
                string tipAfeIgv = "10";
                string codTriIgv = string.Empty;
                string nomTributoIgvItem = string.Empty;
                string codTipTributoIgvItem = string.Empty;
                switch (item.IgvSunat)
                {
                    case "GRAVADO":
                        tipAfeIgv = "10";
                        codTriIgv = "1000";
                        nomTributoIgvItem = "IGV";
                        codTipTributoIgvItem = "VAT";
                        break;
                    case "EXONERADO":
                        tipAfeIgv = "20";
                        codTriIgv = "9997";
                        nomTributoIgvItem = "EXO";
                        codTipTributoIgvItem = "VAT";
                        break;
                    case "GRATUITO":
                        tipAfeIgv = "21";
                        codTriIgv = "9996";
                        nomTributoIgvItem = "GRA";
                        codTipTributoIgvItem = "FRE";
                        break;
                }

                // agregar items al comprobante.
                invoiceDetails.Add(new InvoiceDetail()
                {
                    InvoiceId = invoice,
                    CodUnidadMedida = item.CodUnidadMedida,
                    CtdUnidadItem = item.Quantity,
                    CodProducto = item.ProductId.ToString(),
                    CodProductoSunat = item.CodProductoSunat,
                    DesItem = item.Description,
                    MtoValorUnitario = item.MtoBaseIgvItem,
                    SumTotTributosItem = item.MtoIgvItem,
                    // Tributo: IGV(1000).
                    CodTriIgv = codTriIgv,
                    MtoIgvItem = item.MtoIgvItem,
                    MtoBaseIgvItem = item.MtoBaseIgvItem,
                    NomTributoIgvItem = nomTributoIgvItem,
                    CodTipTributoIgvItem = codTipTributoIgvItem,
                    TipAfeIgv = tipAfeIgv,
                    PorIgvItem = item.IgvSunat == "EXONERADO" ? "0.00" : item.ValorIgv.ToString("N2"),
                    // Tributo ICBPER 7152.
                    CodTriIcbper = item.TriIcbper ? "7152" : "-",
                    MtoTriIcbperItem = item.TriIcbper ? item.MtoTriIcbperItem : 0,
                    CtdBolsasTriIcbperItem = item.TriIcbper ? Convert.ToInt32(item.Quantity) : 0,
                    NomTributoIcbperItem = "ICBPER",
                    CodTipTributoIcbperItem = "OTH",
                    MtoTriIcbperUnidad = item.ValorIcbper,
                    // Precio de Venta Unitario.
                    MtoPrecioVentaUnitario = item.Price,
                    MtoValorVentaItem = item.MtoBaseIgvItem
                });
            });
            return invoiceDetails;
        }

        /// <summary>
        /// Lista de Tributos.
        /// </summary>
        public List<Tributo> GetTributo(int invoiceId)
        {
            bool icbper = false;
            decimal opGravada = 0;
            decimal opExonerada = 0;
            decimal opGratuita = 0;
            decimal totalIgv = 0;
            decimal totalIcbper = 0;
            Details.ForEach(item =>
            {
                switch (item.IgvSunat)
                {
                    case "GRAVADO":
                        opGravada = opGravada + item.MtoBaseIgvItem;
                        totalIgv = totalIgv + item.MtoIgvItem;
                        break;
                    case "EXONERADO":
                        opExonerada = opExonerada + item.MtoBaseIgvItem;
                        break;
                    case "GRATUITO":
                        opGratuita = opGratuita + item.MtoBaseIgvItem;
                        break;
                }

                if (item.TriIcbper)
                {
                    icbper = true;
                    totalIcbper = totalIcbper + item.MtoTriIcbperItem;
                }
            });

            var tributos = new List<Tributo>();
            if (opGratuita > 0)
            {
                tributos.Add(new Tributo()
                {
                    InvoiceId = invoiceId,
                    IdeTributo = "9996", NomTributo = "GRA", CodTipTributo = "FRE",
                    MtoBaseImponible = opGratuita, MtoTributo = 0,
                    Year = DateTime.Now.ToString("yyyy"),
                    Month = DateTime.Now.ToString("MM")
                });
            }

            if (opExonerada > 0)
            {
                tributos.Add(new Tributo()
                {
                    InvoiceId = invoiceId,
                    IdeTributo = "9997", NomTributo = "EXO", CodTipTributo = "VAT",
                    MtoBaseImponible = opExonerada, MtoTributo = 0,
                    Year = DateTime.Now.ToString("yyyy"),
                    Month = DateTime.Now.ToString("MM")
                });
            }

            tributos.Add(new Tributo()
            {
                InvoiceId = invoiceId,
                IdeTributo = "1000", NomTributo = "IGV", CodTipTributo = "VAT",
                MtoBaseImponible = opGravada, MtoTributo = totalIgv,
                Year = DateTime.Now.ToString("yyyy"),
                Month = DateTime.Now.ToString("MM")
            });

            if (icbper)
            {
                tributos.Add(new Tributo()
                {
                    InvoiceId = invoiceId,
                    IdeTributo = "7152", NomTributo = "ICBPER", CodTipTributo = "OTH",
                    MtoBaseImponible = 0, MtoTributo = totalIcbper,
                    Year = DateTime.Now.ToString("yyyy"),
                    Month = DateTime.Now.ToString("MM")
                });
            }

            return tributos;
        }

        /// <summary>
        /// Configurar Cuentas por cobrar.
        /// </summary>
        public List<InvoiceAccount> GetInvoiceAccounts(Invoice invoice)
        {
            string accountType = string.Empty;
            if (invoice.InvoiceType.Equals("VENTA")) accountType = "COBRAR";
            if (invoice.InvoiceType.Equals("COMPRA")) accountType = "PAGAR";
            var invoiceAccounts = new List<InvoiceAccount>();
            Cuotas.ForEach(item =>
            {
                var cuota = new InvoiceAccount()
                {
                    InvoiceId = invoice.Id,
                    Serie = invoice.Serie,
                    Number = invoice.Number,
                    AccountType = accountType,
                    Status = "PENDIENTE",
                    Cuota = item.NumCuota,
                    Amount = item.Amount,
                    Balance = item.Amount,
                    EndDate = item.EndDate,
                    Year = item.EndDate.ToString("yyyy"),
                    Month = item.EndDate.ToString("MM")
                };
                if (item.Id != null) cuota.Id = Guid.Parse(item.Id);
                invoiceAccounts.Add(cuota);
            });
            return invoiceAccounts;
        }
    }
}
