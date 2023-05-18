using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Nebula.Common.Models;

namespace Nebula.Database.Models;

/// <summary>
/// Cuentas por Cobrar.
/// </summary>
[BsonIgnoreExtraElements]
public class Receivable : IGenericModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Tipo Operación: 'CARGO' | 'ABONO'
    /// </summary>
    public string Type { get; set; } = "CARGO";

    /// <summary>
    /// Identificador del Contacto.
    /// </summary>
    public string ContactId { get; set; } = string.Empty;

    /// <summary>
    /// Nombre de Contacto.
    /// </summary>
    public string ContactName { get; set; } = string.Empty;

    /// <summary>
    /// Observación / Comentario.
    /// </summary>
    public string Remark { get; set; } = string.Empty;

    /// <summary>
    /// Clave foránea comprobante de venta.
    /// </summary>
    public string InvoiceSale { get; set; } = "-";

    /// <summary>
    /// Configura el Tipo de comprobante.
    /// </summary>
    public string DocType { get; set; } = "-";

    /// <summary>
    /// Documento de Venta: F001-1.
    /// </summary>
    public string Document { get; set; } = string.Empty;

    /// <summary>
    /// Forma de Pago: Yape | Depósito | Contado.
    /// </summary>
    public string FormaPago { get; set; } = string.Empty;

    /// <summary>
    /// Monto de la deuda.
    /// </summary>
    public decimal Cargo { get; set; }

    /// <summary>
    /// Monto del Pago de la deuda.
    /// </summary>
    public decimal Abono { get; set; }

    /// <summary>
    /// Saldo pendiente a Cobrar.
    /// </summary>
    [BsonIgnore]
    public decimal Saldo { get; set; }

    /// <summary>
    /// Estado del Cargo: 'PENDIENTE' | 'COBRADO' | '-'.
    /// </summary>
    public string Status { get; set; } = "PENDIENTE";

    /// <summary>
    /// Identificador del Terminal de Venta.
    /// </summary>
    public string CajaDiaria { get; set; } = "-";

    /// <summary>
    /// Nombre del Terminal de Venta.
    /// </summary>
    public string Terminal { get; set; } = string.Empty;

    /// <summary>
    /// Identificador del Cargo.
    /// </summary>
    public string ReceivableId { get; set; } = "-";

    /// <summary>
    /// Fecha de Operación.
    /// </summary>
    public string CreatedAt { get; set; } = DateTime.Now.ToString("yyyy-MM-dd");

    /// <summary>
    /// Fecha de Vencimiento a Pagar.
    /// </summary>
    public string EndDate { get; set; } = "-";

    /// <summary>
    /// Año de registro.
    /// </summary>
    public string Year { get; set; } = DateTime.Now.ToString("yyyy");

    /// <summary>
    /// Mes de registro.
    /// </summary>
    public string Month { get; set; } = DateTime.Now.ToString("MM");
}
