using Nebula.Modules.Account.Models;
using Nebula.Modules.Products.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nebula.Modules.Inventory.Models;

public class LocationDetail
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
    /// Identificador de Ubicación.
    /// </summary>
    public Guid? LocationId { get; set; } = null;

    [ForeignKey(nameof(LocationId))]
    public Location Location { get; set; } = new Location();

    /// <summary>
    /// Identificador del Producto.
    /// </summary>
    public Guid? ProductId { get; set; } = null;

    [ForeignKey(nameof(ProductId))]
    public Product Product { get; set; } = new Product();

    /// <summary>
    /// Nombre del Producto.
    /// </summary>
    public string ProductName { get; set; } = string.Empty;

    /// <summary>
    /// Código de Barras.
    /// </summary>
    public string Barcode { get; set; } = string.Empty;

    /// <summary>
    /// Cantidad Máxima de Inventario.
    /// </summary>
    public int QuantityMax { get; set; }

    /// <summary>
    /// Cantidad Mínima de Inventario.
    /// </summary>
    public int QuantityMin { get; set; }
}
