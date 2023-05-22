namespace Nebula.Modules.Cashier.Dto;

public class AperturaCaja
{
    /// <summary>
    /// Id serie facturación.
    /// </summary>
    public string InvoiceSerie { get; set; } = string.Empty;

    /// <summary>
    /// Total monto apertura.
    /// </summary>
    public decimal Total { get; set; }

    /// <summary>
    /// Turno Operación.
    /// </summary>
    public string Turno { get; set; } = string.Empty;
}
