using MongoDB.Bson.Serialization.Attributes;

namespace Nebula.Database.Models.Common;

[BsonIgnoreExtraElements]
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
    /// Modo de Envío SUNAT - para la terminal.
    /// </summary>
    public string ModoEnvioSunat { get; set; } = "FIRMAR";

    /// <summary>
    /// Contacto por defecto para operaciones
    /// menores a 700 soles con boleta.
    /// </summary>
    public string ContactId { get; set; } = string.Empty;

    /// <summary>
    /// #Dias para créditos automáticos.
    /// </summary>
    public int DiasPlazo { get; set; } = 0;

    /// <summary>
    /// String cifrado para verificar Licencia.
    /// </summary>
    public string AccessToken { get; set; } = string.Empty;

    /// <summary>
    /// Identificador para verificar la Suscripción.
    /// </summary>
    public string SubscriptionId { get; set; } = string.Empty;

    #region Consulta de Validez de Comprobante de Pago - API SUNAT

    /// <summary>
    /// Id Credenciales oauth2 - API SUNAT
    /// </summary>
    public string CdPClientId { get; set; } = string.Empty;

    /// <summary>
    /// Clave Credenciales oauth2 - API SUNAT
    /// </summary>
    public string CdPClientSecret { get; set; } = string.Empty;

    #endregion
}
