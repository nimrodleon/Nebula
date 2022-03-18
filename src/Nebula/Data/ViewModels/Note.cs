namespace Nebula.Data.ViewModels;

/// <summary>
/// Nota de Inventario.
/// </summary>
public class Note
{
    /// <summary>
    /// Id de Contacto.
    /// </summary>
    public int ContactId { get; set; }

    /// <summary>
    /// Id del Almacén.
    /// </summary>
    public int WarehouseId { get; set; }

    /// <summary>
    /// Tipo de Nota (Ingreso|Salida).
    /// </summary>
    public string NoteType { get; set; }

    /// <summary>
    /// Motivo de Inventario.
    /// </summary>
    public int Motivo { get; set; }

    /// <summary>
    /// Fecha de registro.
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// Observación de la Nota.
    /// </summary>
    public string Remark { get; set; }

    /// <summary>
    /// Detalle de la Nota.
    /// </summary>
    public List<ItemNote> ItemNotes { get; set; }
}
