using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Nebula.Common.Models;
using System.ComponentModel.DataAnnotations;

namespace Nebula.Modules.Account.Models;

[BsonIgnoreExtraElements]
public class Company : IGenericModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Identificador del dueño de la empresa.
    /// </summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// R.U.C. Empresa.
    /// </summary>
    [Required(ErrorMessage = "El campo 'R.U.C' es obligatorio.")]
    public string Ruc { get; set; } = string.Empty;

    /// <summary>
    /// Razón Social.
    /// </summary>
    [Required(ErrorMessage = "El campo 'Razón Social' es obligatorio.")]
    public string RznSocial { get; set; } = string.Empty;

    /// <summary>
    /// Dirección Empresa.
    /// </summary>
    [Required(ErrorMessage = "El campo 'Dirección' es obligatorio.")]
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
    /// Contacto por defecto para operaciones
    /// menores a 700 soles con boleta.
    /// </summary>
    public string ContactId { get; set; } = string.Empty;

    /// <summary>
    /// #Dias para créditos automáticos.
    /// </summary>
    public int DiasPlazo { get; set; } = 0;
}
