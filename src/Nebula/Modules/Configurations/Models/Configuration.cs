using MongoDB.Bson.Serialization.Attributes;

namespace Nebula.Modules.Configurations.Models;

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

    #region ConfigurationAppsettings

    /// <summary>
    /// URL aplicativo facturador SUNAT.
    /// </summary>
    [BsonIgnore]
    public string? FacturadorUrl { get; set; } = string.Empty;

    /// <summary>
    /// URL SearchPe base de datos contribuyentes.
    /// </summary>
    [BsonIgnore]
    public bool PadronReducidoRuc { get; set; } = false;

    #endregion

    /// <summary>
    /// Habilita/Deshabilita el módulo de punto de venta.
    /// </summary>
    public bool ModTerminal { get; set; } = false;

    /// <summary>
    /// Habilita/Deshabilita el módulo de cuentas por cobrar.
    /// </summary>
    public bool ModReceivable { get; set; } = false;

    /// <summary>
    /// Habilita/Deshabilita el módulo de compras.
    /// </summary>
    public bool ModCompras { get; set; } = false;

    /// <summary>
    /// Habilita/Deshabilita el módulo de inventarios.
    /// </summary>
    public bool ModInventories { get; set; } = false;

    /// <summary>
    /// Habilita/Deshabilita el módulo de taller.
    /// </summary>
    public bool ModTaller { get; set; } = false;

    /// <summary>
    /// Habilita/Deshabilita la gestión de inventarios por lotes.
    /// </summary>
    public bool ModLotes { get; set; } = false;
}
