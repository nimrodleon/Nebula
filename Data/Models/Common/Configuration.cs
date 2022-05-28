using MongoDB.Bson.Serialization.Attributes;

namespace Nebula.Data.Models.Common;

public class Configuration
{
    [BsonId] public string Id { get; set; } = "DEFAULT";

    /// <summary>
    /// R.U.C. Empresa.
    /// </summary>
    public string Ruc { get; set; } = string.Empty;

    /// <summary>
    /// Razón Social.
    /// </summary>
    public string RznSocial { get; set; } = string.Empty;

    /// <summary>
    /// Dirección Empresa.
    /// </summary>
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// Teléfono Empresa.
    /// </summary>
    public string PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    /// Ancho de la impresión del Ticket.
    /// </summary>
    public string AnchoTicket { get; set; } = string.Empty;

    /// <summary>
    /// Código Local Emisor.
    /// </summary>
    public string CodLocalEmisor { get; set; } = string.Empty;

    /// <summary>
    /// Tipo de moneda.
    /// </summary>
    public string TipMoneda { get; set; } = string.Empty;

    /// <summary>
    /// Porcentaje IGV.
    /// </summary>
    public decimal PorcentajeIgv { get; set; }

    /// <summary>
    /// Monto Impuesto a la Bolsa plástica.
    /// </summary>
    public decimal ValorImpuestoBolsa { get; set; }

    /// <summary>
    /// Tipo de Emisión Electrónica.
    /// </summary>
    public string CpeSunat { get; set; } = string.Empty;

    /// <summary>
    /// Contacto por defecto para operaciones
    /// menores a 700 soles con boleta.
    /// </summary>
    public string ContactId { get; set; } = string.Empty;

    /// <summary>
    /// URL Api. CPE - SUNAT.
    /// </summary>
    public string UrlApi { get; set; } = string.Empty;

    /// <summary>
    /// Path Archivos SUNAT.
    /// </summary>
    public string FileSunat { get; set; } = string.Empty;

    /// <summary>
    /// Path Archivos de Control.
    /// </summary>
    public string FileControl { get; set; } = string.Empty;
}
