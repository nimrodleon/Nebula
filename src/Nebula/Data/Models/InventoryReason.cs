using System.ComponentModel.DataAnnotations;

namespace Nebula.Data.Models;

/// <summary>
/// TODO: refactoring.
/// Motivos para tipos de inventario.
/// </summary>
public class InventoryReason
{
    public int Id { get; set; }

    /// <summary>
    /// Descripción de Motivo.
    /// </summary>
    [MaxLength(250)]
    public string Description { get; set; }

    /// <summary>
    /// Transfer|Input|Output
    /// </summary>
    [MaxLength(250)]
    public string Type { get; set; }
}
