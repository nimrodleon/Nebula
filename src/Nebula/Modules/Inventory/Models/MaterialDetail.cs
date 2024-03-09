using System.ComponentModel.DataAnnotations;
using Nebula.Modules.Account.Models;
using System.ComponentModel.DataAnnotations.Schema;
using Nebula.Modules.Products.Models;

namespace Nebula.Modules.Inventory.Models;

public class MaterialDetail
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
    /// Clave Foranea del Material.
    /// </summary>
    public Guid? MaterialId { get; set; } = null;

    [ForeignKey(nameof(MaterialId))]
    public Material Material { get; set; } = new Material();

    /// <summary>
    /// Identificador del Almacén.
    /// </summary>
    public Guid? WarehouseId { get; set; } = null;

    [ForeignKey(nameof(WarehouseId))]
    public Warehouse Warehouse { get; set; } = new Warehouse();

    /// <summary>
    /// Nombre del Almacén.
    /// </summary>
    public string WarehouseName { get; set; } = string.Empty;

    /// <summary>
    /// Cantidad de Salida.
    /// </summary>
    public int CantSalida { get; set; }

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
    /// Cantidad de Retorno.
    /// </summary>
    public int CantRetorno { get; set; }

    /// <summary>
    /// Cantidad Usado.
    /// </summary>
    public int CantUsado { get; set; }

    /// <summary>
    /// Fecha de Creación.
    /// </summary>
    public string CreatedAt { get; set; } = DateTime.Now.ToString("yyyy-MM-dd");
}
