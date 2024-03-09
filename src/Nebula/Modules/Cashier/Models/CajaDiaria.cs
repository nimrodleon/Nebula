using Nebula.Modules.Account.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nebula.Modules.Cashier.Models;

public class CajaDiaria
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Identificador de la empresa al que pertenece.
    /// </summary>
    public Guid? CompanyId { get; set; } = null;

    [ForeignKey(nameof(CompanyId))]
    public Company Company { get; set; } = new Company();

    /// <summary>
    /// ID Serie de facturación.
    /// </summary>
    public Guid? InvoiceSerieId { get; set; } = null;

    [ForeignKey(nameof(InvoiceSerieId))]
    public InvoiceSerie InvoiceSerie { get; set; } = new InvoiceSerie();

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

    /// <summary>
    /// Identificador del Almacén.
    /// Esta propiedad se usa solo para mostrar datos.
    /// </summary>
    public Guid? WarehouseId { get; set; } = null;

    [ForeignKey(nameof(WarehouseId))]
    public Warehouse Warehouse { get; set; } = new Warehouse();
}
