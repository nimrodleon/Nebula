using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Nebula.Common.Models;

namespace Nebula.Database.Models.Sales;

[BsonIgnoreExtraElements]
public class TributoCreditNote : IGenericModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// foreignKey in db.
    /// </summary>
    public string CreditNoteId { get; set; } = string.Empty;

    /// <summary>
    /// Identificador de tributo.
    /// </summary>
    public string IdeTributo { get; set; } = string.Empty;

    /// <summary>
    /// Nombre de tributo.
    /// </summary>
    public string NomTributo { get; set; } = string.Empty;

    /// <summary>
    /// Código de tipo de tributo.
    /// </summary>
    public string CodTipTributo { get; set; } = string.Empty;

    /// <summary>
    /// Base imponible.
    /// </summary>
    public decimal MtoBaseImponible { get; set; }

    /// <summary>
    /// Monto de Tributo.
    /// </summary>
    public decimal MtoTributo { get; set; }

    /// <summary>
    /// Año de registro.
    /// </summary>
    public string Year { get; set; } = DateTime.Now.ToString("yyyy");

    /// <summary>
    /// Mes de registro.
    /// </summary>
    public string Month { get; set; } = DateTime.Now.ToString("MM");
}
