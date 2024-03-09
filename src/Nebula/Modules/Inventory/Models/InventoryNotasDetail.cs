using System.ComponentModel.DataAnnotations;
using Nebula.Modules.Account.Models;
using System.ComponentModel.DataAnnotations.Schema;
using Nebula.Modules.Products.Models;

namespace Nebula.Modules.Inventory.Models;

public class InventoryNotasDetail
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
    /// Clave foranea NotaId.
    /// </summary>
    public Guid? InventoryNotasId { get; set; } = null;

    [ForeignKey(nameof(InventoryNotasId))]
    public InventoryNotas InventoryNotas { get; set; } = new InventoryNotas();

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
    /// Cantidad Requerida.
    /// </summary>
    public decimal Demanda { get; set; }
}
