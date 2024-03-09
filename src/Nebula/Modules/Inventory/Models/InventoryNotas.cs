using Nebula.Modules.Account.Models;
using Nebula.Modules.Contacts.Models;
using Nebula.Modules.Inventory.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nebula.Modules.Inventory.Models;

public class InventoryNotas
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
    /// Nombre de Usuario.
    /// </summary>
    public string User { get; set; } = string.Empty;

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
    /// Identificador de Contacto.
    /// </summary>
    public Guid? ContactId { get; set; } = null;

    [ForeignKey(nameof(ContactId))]
    public Contact Contact { get; set; } = new Contact();

    /// <summary>
    /// Nombre de Contacto.
    /// </summary>
    public string ContactName { get; set; } = string.Empty;

    /// <summary>
    /// Tipo de Inventario. ENTRADA | SALIDA
    /// </summary>
    public string TransactionType { get; set; } = Helpers.TransactionType.ENTRADA;

    /// <summary>
    /// Estado del Inventario.
    /// </summary>
    public string Status { get; set; } = InventoryStatus.BORRADOR;

    /// <summary>
    /// Observación.
    /// </summary>
    public string Remark { get; set; } = string.Empty;

    /// <summary>
    /// Fecha de Registro.
    /// </summary>
    public string CreatedAt { get; set; } = DateTime.Now.ToString("yyyy-MM-dd");

    /// <summary>
    /// Año de Registro.
    /// </summary>
    public string Year { get; set; } = DateTime.Now.ToString("yyyy");

    /// <summary>
    /// Mes de Registro.
    /// </summary>
    public string Month { get; set; } = DateTime.Now.ToString("MM");
}
