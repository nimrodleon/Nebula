using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nebula.Data.Models;

// TODO: refactoring.
public class InventoryNote
{
    public int Id { get; set; }

    /// <summary>
    /// Id de Contacto.
    /// </summary>
    public int? ContactId { get; set; }

    /// <summary>
    /// Clave foránea Contacto.
    /// </summary>
    [ForeignKey("ContactId")]
    public Contact Contact { get; set; }

    /// <summary>
    /// Id Almacén.
    /// </summary>
    public int? WarehouseId { get; set; }

    /// <summary>
    /// Clave foránea Almacén.
    /// </summary>
    [ForeignKey("WarehouseId")]
    public Warehouse Warehouse { get; set; }

    /// <summary>
    /// Tipo de Nota (Ingreso|Salida).
    /// </summary>
    [MaxLength(250)]
    public string NoteType { get; set; }

    /// <summary>
    /// Motivo de Inventario.
    /// </summary>
    [MaxLength(250)]
    public string Motivo { get; set; }

    /// <summary>
    /// Fecha de Registro.
    /// </summary>
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }

    /// <summary>
    /// Estado de la Nota.
    /// </summary>
    [MaxLength(250)]
    public string Status { get; set; }

    /// <summary>
    /// Año de Inventario.
    /// </summary>
    [MaxLength(250)]
    public string Year { get; set; }

    /// <summary>
    /// Mes de Inventario.
    /// </summary>
    [MaxLength(250)]
    public string Month { get; set; }

    /// <summary>
    /// Detalles Nota de Inventario.
    /// </summary>
    public List<InventoryNoteDetail> InventoryNoteDetails { get; set; }
}
