using Nebula.Modules.Account.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nebula.Modules.Taller.Models;

/// <summary>
/// Item de materiales usados en la orden de reparación.
/// </summary>
public class TallerItemRepairOrder
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Identificador de la empresa al que pertenece.
    /// </summary>
    public Guid? CompanyId { get; set; } = null;

    [ForeignKey(nameof(CompanyId))]
    public Company Company { get; set; } = new Company();

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
