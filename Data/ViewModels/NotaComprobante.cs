using System;
using System.Collections.Generic;
using Nebula.Data.Models;

namespace Nebula.Data.ViewModels
{
    /// <summary>
    /// modelo para la emisión de notas crédito/débito.
    /// </summary>
    public class NotaComprobante
    {
        /// <summary>
        /// Id del Comprobante.
        /// </summary>
        public int InvoiceId { get; set; }

        /// <summary>
        /// Fecha de registro.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Tipo documento NOTA: (CRÉDITO/DÉBITO) => (NC/ND).
        /// </summary>
        public string DocType { get; set; }

        /// <summary>
        /// Código del Motivo de emisión.
        /// </summary>
        public string CodMotivo { get; set; }

        /// <summary>
        /// Serie Comprobante #SOLO PARA COMPRAS.
        /// </summary>
        public string Serie { get; set; }

        /// <summary>
        /// Número Comprobante #SOLO PARA COMPRAS.
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Descripción del motivo.
        /// </summary>
        public string DesMotivo { get; set; }

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
        /// Detalle de Venta.
        /// </summary>
        public List<CpeDetail> Details { get; set; }

        /// <summary>
        /// calcular importe de venta.
        /// </summary>
        private void CalcImporteVenta()
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
        /// Configurar cabecera de Nota.
        /// </summary>
        public InvoiceNote GetInvoiceNote(Invoice invoice)
        {
            CalcImporteVenta();

        }

        /// <summary>
        /// Configurar Detalle de Notas Crédito/Débito.
        /// </summary>
        public List<InvoiceNoteDetail> GetInvoiceNoteDetail(int invoiceNoteId)
        {
            var invoiceNoteDetails = new List<InvoiceNoteDetail>();
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
                invoiceNoteDetails.Add(new InvoiceNoteDetail()
                {
                    InvoiceNoteId = invoiceNoteId,
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
            return invoiceNoteDetails;
        }
    }
}
