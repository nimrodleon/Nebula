using System;
using System.Collections.Generic;
using Nebula.Data.Models;

namespace Nebula.Data.ViewModels
{
    /// <summary>
    /// modelo para el punto de venta.
    /// </summary>
    public class Sale
    {
        /// <summary>
        /// Id del Contacto.
        /// </summary>
        public int ClientId { get; set; }

        /// <summary>
        /// Medios de Pago.
        /// </summary>
        public int PaymentMethod { get; set; }

        /// <summary>
        /// Tipo documento.
        /// </summary>
        public string DocType { get; set; }

        /// <summary>
        /// Monto Cobrado.
        /// </summary>
        public decimal? MontoTotal { get; set; }

        /// <summary>
        /// Vuelto para el Cliente.
        /// </summary>
        public decimal? Vuelto { get; set; }

        /// <summary>
        /// SubTotal.
        /// </summary>
        public decimal? SumTotValVenta { get; set; }

        /// <summary>
        /// Sumatoria Tributos.
        /// </summary>
        public decimal? SumTotTributos { get; set; }

        /// <summary>
        /// Importe a Cobrar.
        /// </summary>
        public decimal? SumImpVenta { get; set; }

        /// <summary>
        /// Observación.
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// Detalle de Venta.
        /// </summary>
        public List<SaleDetail> Details { get; set; }

        /// <summary>
        /// calcular importe de venta.
        /// </summary>
        public void CalcImporteVenta()
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
        /// Lista de Tributos.
        /// </summary>
        public List<Tributo> GetTributo()
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
                    IdeTributo = "9997", NomTributo = "EXO", CodTipTributo = "VAT",
                    MtoBaseImponible = opExonerada, MtoTributo = 0,
                    Year = DateTime.Now.ToString("yyyy"),
                    Month = DateTime.Now.ToString("MM")
                });
            }

            tributos.Add(new Tributo()
            {
                IdeTributo = "1000", NomTributo = "IGV", CodTipTributo = "VAT",
                MtoBaseImponible = opGravada, MtoTributo = totalIgv,
                Year = DateTime.Now.ToString("yyyy"),
                Month = DateTime.Now.ToString("MM")
            });

            if (icbper)
            {
                tributos.Add(new Tributo()
                {
                    IdeTributo = "7152", NomTributo = "ICBPER", CodTipTributo = "OTH",
                    MtoBaseImponible = 0, MtoTributo = totalIcbper,
                    Year = DateTime.Now.ToString("yyyy"),
                    Month = DateTime.Now.ToString("MM")
                });
            }

            return tributos;
        }
    }
}
