namespace Nebula.Data.ViewModels.Cashier;

public class AperturaCaja
{
    /// <summary>
    /// Id de la serie facturación.
    /// </summary>
    public string SerieId { get; set; } = string.Empty;

    /// <summary>
    /// Total monto apertura.
    /// </summary>
    public decimal Total { get; set; }

    /// <summary>
    /// Turno Operación.
    /// </summary>
    public string Turno { get; set; } = string.Empty;
}
