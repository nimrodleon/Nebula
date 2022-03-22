namespace Nebula.Data.ViewModels.Cashier;

/// <summary>
/// Cuota de Créditos.
/// </summary>
public class Cuota
{
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Número de cuota.
    /// </summary>
    public int NumCuota { get; set; }

    /// <summary>
    /// Fecha de vencimiento.
    /// </summary>
    public string EndDate { get; set; } = string.Empty;

    /// <summary>
    /// Monto prometido.
    /// </summary>
    public decimal Amount { get; set; }
}
