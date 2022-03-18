namespace Nebula.Data.ViewModels;

public class AperturaCaja
{
    /// <summary>
    /// Id de la serie facturación.
    /// </summary>
    public string SerieId { get; set; }

    /// <summary>
    /// Total monto apertura.
    /// </summary>
    public decimal Total { get; set; }

    /// <summary>
    /// Turno Operación.
    /// </summary>
    public string Turno { get; set; }
}
