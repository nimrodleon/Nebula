using System.ComponentModel.DataAnnotations;
using Nebula.Modules.Account.Models;
using System.ComponentModel.DataAnnotations.Schema;
using Nebula.Modules.Products.Models;

namespace Nebula.Modules.Inventory.Models;

public class AjusteInventarioDetail
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
    /// Clave foranea Ajuste Inventario.
    /// </summary>
    public Guid? AjusteInventarioId { get; set; } = null;

    [ForeignKey(nameof(AjusteInventarioId))]
    public AjusteInventario AjusteInventario { get; set; } = new AjusteInventario();

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
    /// Cantidad Existente.
    /// </summary>
    public decimal CantExistente { get; set; }

    /// <summary>
    /// Cantidad Contada.
    /// </summary>
    public long CantContada { get; set; }
}
