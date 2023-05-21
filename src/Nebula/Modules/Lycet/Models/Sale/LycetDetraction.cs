namespace Nebula.Modules.Lycet.Models.Sale;

public class LycetDetraction
{
    /// <summary>
    /// Porcentaje de la detracción.
    /// </summary>
    public decimal Percent { get; set; }

    /// <summary>
    /// Monto de la detracción.
    /// </summary>
    public decimal Mount { get; set; }

    public string CtaBanco { get; set; } = string.Empty;
    public string CodMedioPago { get; set; } = string.Empty;
    public string CodBienDetraccion { get; set; } = string.Empty;

    /// <summary>
    /// Valor referencial, en el caso de detracciones
    /// al transporte de bienes por vía terrestre.
    /// </summary>
    public decimal ValueRef { get; set; }
}
