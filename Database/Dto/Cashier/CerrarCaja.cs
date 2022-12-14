namespace Nebula.Database.Dto.Cashier;

public class CerrarCaja
{
    /// <summary>
    /// Monto contabilizado en caja.
    /// </summary>
    public decimal TotalContabilizado { get; set; }

    /// <summary>
    /// Monto para el siguiente turno.
    /// </summary>
    public decimal TotalCierre { get; set; }
}
