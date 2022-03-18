namespace Nebula.Data.ViewModels;

/// <summary>
/// Nota Transferencia entre almacenes.
/// </summary>
public class Transfer
{
    /// <summary>
    /// Id Almacén de origen.
    /// </summary>
    public int Origin { get; set; }

    /// <summary>
    /// Id Almacén destino.
    /// </summary>
    public int Target { get; set; }

    /// <summary>
    /// Motivo del Traslado.
    /// </summary>
    public int Motivo { get; set; }

    /// <summary>
    /// Fecha de registro.
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// Detalle de Transferencia.
    /// </summary>
    public List<ItemNote> ItemNotes { get; set; }
}
