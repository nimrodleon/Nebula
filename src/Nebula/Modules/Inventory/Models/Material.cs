using Nebula.Modules.Inventory.Helpers;
using System.ComponentModel.DataAnnotations;
using Nebula.Modules.Account.Models;
using System.ComponentModel.DataAnnotations.Schema;
using Nebula.Modules.Contacts.Models;

namespace Nebula.Modules.Inventory.Models;

public class Material
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
    /// Usuario Autentificado.
    /// </summary>
    public string User { get; set; } = string.Empty;

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
    /// Identificador del Trabajador.
    /// </summary>
    public Guid? EmployeeId { get; set; } = null;

    [ForeignKey(nameof(EmployeeId))]
    public Contact Employee { get; set; } = new Contact();

    /// <summary>
    /// Nombre del Trabajador.
    /// </summary>
    public string EmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// Estado del Inventario.
    /// </summary>
    public string Status { get; set; } = InventoryStatus.BORRADOR;

    /// <summary>
    /// Comentario del Inventario.
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
