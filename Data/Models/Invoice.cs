using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nebula.Data.Models
{
    public class Invoice
    {
        public int Id { get; set; }

        /// <summary>
        /// Tipo documento para control interno.
        /// FACTURA|BOLETA|NOTA DE VENTA, (FT|BL|NV).
        /// </summary>
        [MaxLength(250)]
        public string TypeDoc { get; set; }

        /// <summary>
        /// Tipo factura (Compra|Venta)
        /// </summary>
        [MaxLength(250)]
        public string InvoiceType { get; set; }

        /// <summary>
        /// Serie comprobante.
        /// </summary>
        [MaxLength(250)]
        public string Serie { get; set; }

        /// <summary>
        /// Número comprobante.
        /// </summary>
        [MaxLength(250)]
        public string Number { get; set; }

        /// <summary>
        /// Tipo de operación Catálogo: 51, n4
        /// </summary>
        [MaxLength(250)]
        public string TipOperacion { get; set; }

        /// <summary>
        /// fecha de emisión. Formato: YYYY-MM-DD, an..10
        /// </summary>
        [MaxLength(250)]
        public string FecEmision { get; set; }

        /// <summary>
        /// hora emisión. Formato: HH:MM:SS, an..14
        /// </summary>
        [MaxLength(250)]
        public string HorEmision { get; set; }

        /// <summary>
        /// fecha de vencimiento. Formato: YYYY-MM-DD, an..10
        /// Sin Fecha: Por defecto guión -
        /// </summary>
        [MaxLength(250)]
        public string FecVencimiento { get; set; }

        /// <summary>
        /// Forma de pago. Credito / Contado - a7
        /// </summary>
        [MaxLength(250)]
        public string FormaPago { get; set; }

        /// <summary>
        /// Tipo de documento de identidad del adquirente o usuario. Catálogo: 6, an1
        /// </summary>
        [MaxLength(250)]
        public string TipDocUsuario { get; set; }

        /// <summary>
        /// Número de documento de identidad del adquirente o usuario. an..15
        /// </summary>
        [MaxLength(250)]
        public string NumDocUsuario { get; set; }

        /// <summary>
        /// Apellidos y nombres, denominación o razón social del adquirente o usuario. an..1500
        /// </summary>
        [MaxLength(1500)]
        public string RznSocialUsuario { get; set; }

        /// <summary>
        /// Tipo de moneda en la cual se emite la factura electrónica. Catálogo: 2, an3
        /// </summary>
        [MaxLength(250)]
        public string TipMoneda { get; set; }

        /// <summary>
        /// Sumatoria Tributos. an..15|n(12,2)
        /// </summary>
        public decimal? SumTotTributos { get; set; }

        /// <summary>
        /// Total valor de venta. an..15|n(12,2)
        /// </summary>
        public decimal? SumTotValVenta { get; set; }

        /// <summary>
        /// Total Precio de Venta. an..15|n(12,2)
        /// </summary>
        public decimal? SumPrecioVenta { get; set; }

        /// <summary>
        /// Total descuentos (no afectan la base imponible del IGV/IVAP). an..15|n(12,2)
        /// </summary>
        public decimal? SumDescTotal { get; set; }

        /// <summary>
        /// Sumatoria otros Cargos. an..15|n(12,2)
        /// </summary>
        public decimal? SumOtrosCargos { get; set; }

        /// <summary>
        /// Total Anticipos. an..15|n(12,2)
        /// </summary>
        public decimal? SumTotalAnticipos { get; set; }

        /// <summary>
        /// Importe total de la venta, cesión en uso o del servicio prestado. an..15|n(12,2)
        /// </summary>
        public decimal? SumImpVenta { get; set; }

        /// <summary>
        /// Año de registro.
        /// </summary>
        [MaxLength(250)]
        public string Year { get; set; }

        /// <summary>
        /// Mes de registro.
        /// </summary>
        [MaxLength(250)]
        public string Month { get; set; }

        /// <summary>
        /// Lista de items de la factura.
        /// </summary>
        public List<InvoiceDetail> InvoiceDetails { get; set; }

        /// <summary>
        /// Cuentas por Cobrar/Pagar.
        /// </summary>
        public List<InvoiceAccount> InvoiceAccounts { get; set; }
    }
}
