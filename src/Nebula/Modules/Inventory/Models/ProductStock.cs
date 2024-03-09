using Nebula.Modules.Account.Models;
using Nebula.Modules.Products.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nebula.Modules.Inventory.Models;

public class ProductStock
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
    /// Identificador Almacén.
    /// </summary>
    public Guid? WarehouseId { get; set; } = null;

    [ForeignKey(nameof(WarehouseId))]
    public Warehouse Warehouse { get; set; } = new Warehouse();

    /// <summary>
    /// Identificador Producto.
    /// </summary>
    public Guid? ProductId { get; set; } = null;

    [ForeignKey(nameof(ProductId))]
    public Product Product { get; set; } = new Product();

    /// <summary>
    /// Tipo de Transacción de Inventario (Entrada o Salida).
    /// </summary>
    public string TransactionType { get; set; } = Helpers.TransactionType.ENTRADA;

    /// <summary>
    /// Cantidad de Productos.
    /// </summary>
    public decimal Quantity { get; set; } = decimal.Zero;
}
