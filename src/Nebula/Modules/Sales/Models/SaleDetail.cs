using Nebula.Modules.Account.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nebula.Modules.Sales.Models;

public abstract class SaleDetail
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid? CompanyId { get; set; } = null;
    [ForeignKey(nameof(CompanyId))]
    public Company Company { get; set; } = new Company();
    public string WarehouseId { get; set; } = string.Empty;
    public string TipoItem { get; set; } = "BIEN";
    public string Unidad { get; set; } = string.Empty;
    public decimal Cantidad { get; set; }
    public string CodProducto { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal MtoValorUnitario { get; set; }
    public decimal MtoBaseIgv { get; set; }
    public decimal PorcentajeIgv { get; set; }
    public decimal Igv { get; set; }
    public string TipAfeIgv { get; set; } = string.Empty;
    public decimal TotalImpuestos { get; set; }
    public decimal MtoPrecioUnitario { get; set; }
    public decimal MtoValorVenta { get; set; }
    public string RecordType { get; set; } = "PRODUCTO";
    public string Year { get; set; } = DateTime.Now.ToString("yyyy");
    public string Month { get; set; } = DateTime.Now.ToString("MM");
}
