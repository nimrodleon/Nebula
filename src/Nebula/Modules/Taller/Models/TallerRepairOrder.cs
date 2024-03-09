using Nebula.Modules.Account.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nebula.Modules.Taller.Models;

/// <summary>
/// Orden de Reparación.
/// </summary>
public class TallerRepairOrder
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Identificador de la empresa al que pertenece.
    /// </summary>
    public Guid? CompanyId { get; set; } = null;

    [ForeignKey(nameof(CompanyId))]
    public Company Company { get; set; } = new Company();

    #region Serie y Número Correlativo

    public string Serie { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;

    #endregion

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
