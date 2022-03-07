using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nebula.Data.Models
{
    // TODO: refactoring.
    public class InvoiceNote
    {
        public int Id { get; set; }

        /// <summary>
        /// Identificador Comprobante.
        /// </summary>
        public int? InvoiceId { get; set; }

        /// <summary>
        /// Tipo doc. NOTA: (CRÉDITO/DÉBITO) => (NC/ND).
        /// </summary>
        [MaxLength(250)]
        public string DocType { get; set; }

        /// <summary>
        /// Tipo factura (Compra|Venta)
        /// </summary>
        [MaxLength(250)]
        public string InvoiceType { get; set; }

        /// <summary>
        /// Serie Nota.
        /// </summary>
        [MaxLength(250)]
        public string Serie { get; set; }

        /// <summary>
        /// Número Nota.
        /// </summary>
        [MaxLength(250)]
        public string Number { get; set; }

        /// <summary>
        /// Tipo de operación. Catálogo: 51, n2
        /// </summary>
        [MaxLength(250)]
        public string TipOperacion { get; set; }

        /// <summary>
        /// Fecha de emisión. Formato: YYYY-MM-DD, an..10
        /// </summary>
        [MaxLength(250)]
        public string FecEmision { get; set; }

        /// <summary>
        /// Hora de Emisión. Formato: HH:MM:SS, an..14
        /// </summary>
        [MaxLength(250)]
        public string HorEmision { get; set; }

        /// <summary>
        /// Código del domicilio fiscal o de local anexo del emisor. n3
        /// </summary>
        [MaxLength(250)]
        public string CodLocalEmisor { get; set; }

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
        /// Apellidos y nombres, denominación o razón social del adquirente o usuario. an..100
        /// </summary>
        [MaxLength(250)]
        public string RznSocialUsuario { get; set; }

        /// <summary>
        /// Tipo de moneda en la cual se emite la factura electrónica. Catálogo: 2, an3
        /// </summary>
        [MaxLength(250)]
        public string TipMoneda { get; set; }

        /// <summary>
        /// Código del tipo de Nota  electrónica. Catálogo: 10, an2
        /// </summary>
        [MaxLength(250)]
        public string CodMotivo { get; set; }

        /// <summary>
        /// Descripción de motivo o sustento. an..250
        /// </summary>
        [MaxLength(250)]
        public string DesMotivo { get; set; }

        /// <summary>
        /// Tipo de documento del documento que modifica. 01 o 03 o 12, an2
        /// </summary>
        [MaxLength(250)]
        public string TipDocAfectado { get; set; }

        /// <summary>
        /// Serie y número del documento que modifica. Formato: XXXX-99999999, n..13
        /// </summary>
        [MaxLength(250)]
        public string NumDocAfectado { get; set; }

        /// <summary>
        /// Sumatoria Tributos. Formato: n(12,2), an..15
        /// </summary>
        public decimal? SumTotTributos { get; set; }

        /// <summary>
        /// Total valor de venta. Formato: n(12,2), an..15
        /// </summary>
        public decimal? SumTotValVenta { get; set; }

        /// <summary>
        /// Total Precio de Venta. Formato: n(12,2), an..15
        /// </summary>
        public decimal? SumPrecioVenta { get; set; }

        /// <summary>
        /// Importe total de la venta, cesión en uso o del servicio prestado. Formato: n(12,2), an..15
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
        /// Lista de items notas de crédito/débito.
        /// </summary>
        public List<InvoiceNoteDetail> InvoiceNoteDetails { get; set; }
    }
}
