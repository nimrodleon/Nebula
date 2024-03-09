using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nebula.Modules.Account.Models;

public class Warehouse
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
    /// Nombre Almacén.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Observación.
    /// </summary>
    public string Remark { get; set; } = string.Empty;
}
