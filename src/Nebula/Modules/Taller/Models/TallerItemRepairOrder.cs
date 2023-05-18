using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Nebula.Database.Models;

namespace Nebula.Modules.Taller.Models;

/// <summary>
/// Item de materiales usados en la orden de reparación.
/// </summary>
[BsonIgnoreExtraElements]
public class TallerItemRepairOrder : IGeneric
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    public string RepairOrderId { get; set; } = string.Empty;

    #region Datos Almacén

    public string WarehouseId { get; set; } = string.Empty;
    public string WarehouseName { get; set; } = string.Empty;

    #endregion

    public int Quantity { get; set; } = 0;
    public decimal PrecioUnitario { get; set; } = 0;

    #region Datos del Producto

    public string ProductId { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    #endregion

    public decimal Monto { get; set; } = 0;
}
