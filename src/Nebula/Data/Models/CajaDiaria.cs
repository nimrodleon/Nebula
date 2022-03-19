using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Nebula.Data.Models;

public class CajaDiaria
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Series de facturación.
    /// </summary>
    public string Terminal { get; set; } = string.Empty;

    /// <summary>
    /// Estado Caja (ABIERTO|CERRADO).
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Monto Apertura.
    /// </summary>
    public decimal TotalApertura { get; set; }

    /// <summary>
    /// Monto Contabilizado durante el dia.
    /// </summary>
    public decimal TotalContabilizado { get; set; }

    /// <summary>
    /// Monto para el dia siguiente.
    /// </summary>
    public decimal TotalCierre { get; set; }

    /// <summary>
    /// Turno Operación de caja.
    /// </summary>
    public string Turno { get; set; } = string.Empty;

    /// <summary>
    /// Fecha de Operación.
    /// </summary>
    public string CreatedAt { get; set; } = DateTime.Now.ToString("yyyy-MM-dd");

    /// <summary>
    /// Año de registro.
    /// </summary>
    public string Year { get; set; } = DateTime.Now.ToString("yyyy");

    /// <summary>
    /// Mes de registro.
    /// </summary>
    public string Month { get; set; } = DateTime.Now.ToString("MM");
}
