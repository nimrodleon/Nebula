using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nebula.Modules.Account.Models;

/// <summary>
/// Series de facturación.
/// </summary>
public class InvoiceSerie
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
    /// Identificador Serie.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Identificador del Almacén.
    /// </summary>
    public Guid? WarehouseId { get; set; } = null;

    [ForeignKey(nameof(WarehouseId))]
    public Warehouse Warehouse { get; set; } = new Warehouse();

    /// <summary>
    /// Serie Nota de Venta.
    /// </summary>
    public string NotaDeVenta { get; set; } = string.Empty;

    /// <summary>
    /// Contador Nota de Venta.
    /// </summary>
    public int CounterNotaDeVenta { get; set; }

    /// <summary>
    /// Serie Boleta.
    /// </summary>
    public string Boleta { get; set; } = string.Empty;

    /// <summary>
    /// Contador Boleta.
    /// </summary>
    public int CounterBoleta { get; set; }

    /// <summary>
    /// Serie Factura.
    /// </summary>
    public string Factura { get; set; } = string.Empty;

    /// <summary>
    /// Contador Factura.
    /// </summary>
    public int CounterFactura { get; set; }

    /// <summary>
    /// Serie Nota de Crédito Boleta.
    /// </summary>
    public string CreditNoteBoleta { get; set; } = string.Empty;

    /// <summary>
    /// Contador Nota de Crédito Boleta.
    /// </summary>
    public int CounterCreditNoteBoleta { get; set; }

    /// <summary>
    /// Serie Nota de Crédito Factura.
    /// </summary>
    public string CreditNoteFactura { get; set; } = string.Empty;

    /// <summary>
    /// Contador Nota de Crédito Factura.
    /// </summary>
    public int CounterCreditNoteFactura { get; set; }
}
