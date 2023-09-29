using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Nebula.Common.Models;

namespace Nebula.Modules.Sales.Models;

[BsonIgnoreExtraElements]
public class TributoSale : IGenericModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Identificador de la empresa al que pertenece.
    /// </summary>
    public string CompanyId { get; set; } = string.Empty;

    /// <summary>
    /// foreignKey in db.
    /// </summary>
    public string InvoiceSaleId { get; set; } = string.Empty;

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
