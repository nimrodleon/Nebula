using Nebula.Modules.Account.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nebula.Modules.Contacts.Models;

public class Contact
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Identificador de la empresa al que pertenece.
    /// </summary>
    [Required(ErrorMessage = "CompanyId es requerido.")]
    public Guid? CompanyId { get; set; } = null;

    [ForeignKey(nameof(CompanyId))]
    public Company Company { get; set; } = new Company();

    /// <summary>
    /// Documento de Identidad.
    /// </summary>
    [Required(ErrorMessage = "Documento es requerido.")]
    public string Document { get; set; } = string.Empty;

    /// <summary>
    /// Tipo de documento.
    /// </summary>
    [Required(ErrorMessage = "Tipo documento es requerido.")]
    public string DocType { get; set; } = string.Empty;

    /// <summary>
    /// Nombre de contacto.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Dirección de contacto.
    /// </summary>
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// Número Telefónico.
    /// </summary>
    public string PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    /// Dirección del contacto (Código de ubigeo).
    /// </summary>
    public string CodUbigeo { get; set; } = string.Empty;
}
