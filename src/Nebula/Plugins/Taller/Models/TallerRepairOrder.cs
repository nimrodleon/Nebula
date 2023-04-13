using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Nebula.Database.Models;

namespace Nebula.Plugins.Taller.Models;

/// <summary>
/// Orden de Reparación.
/// </summary>
[BsonIgnoreExtraElements]
public class TallerRepairOrder : IGeneric
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    #region Datos del Cliente

    public string IdCliente { get; set; } = string.Empty;
    public string NombreCliente { get; set; } = string.Empty;

    #endregion

    public string DatosEquipo { get; set; } = string.Empty;
    public string TareaRealizar { get; set; } = string.Empty;

    #region Datos Almacén

    public string WarehouseId { get; set; } = string.Empty;
    public string WarehouseName { get; set; } = string.Empty;

    #endregion

    #region Técnico Asignado

    public string TechnicalId { get; set; } = string.Empty;
    public string TechnicalName { get; set; } = string.Empty;

    #endregion

    public string Status { get; set; } = TallerRepairOrderStatus.Pendiente;
    public string InvoiceSerieId { get; set; } = string.Empty;
    public decimal RepairAmount { get; set; } = 0;

    #region FechasDeRegistros

    public string CreatedAt { get; set; } = DateTime.Now.ToString("yyyy-MM-dd");
    public string UpdatedAt { get; set; } = DateTime.Now.ToString("yyyy-MM-dd");
    public string Year { get; set; } = DateTime.Now.ToString("yyyy");
    public string Month { get; set; } = DateTime.Now.ToString("MM");

    #endregion
}
