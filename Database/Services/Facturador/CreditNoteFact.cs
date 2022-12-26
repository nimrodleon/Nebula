namespace Nebula.Database.Services.Facturador;

public class CreditNoteFact
{
    public string tipOperacion { get; set; } = string.Empty;
    public string fecEmision { get; set; } = string.Empty;
    public string horEmision { get; set; } = string.Empty;
    public string? codLocalEmisor { get; set; } = null;
    public string tipDocUsuario { get; set; } = string.Empty;
    public string numDocUsuario { get; set; } = string.Empty;
    public string rznSocialUsuario { get; set; } = string.Empty;
    public string tipMoneda { get; set; } = string.Empty;
    public string codMotivo { get; set; } = string.Empty;
    public string desMotivo { get; set; } = string.Empty;
    public string tipDocAfectado { get; set; } = string.Empty;
    public string numDocAfectado { get; set; } = string.Empty;
    public string sumTotTributos { get; set; } = "0.00";
    public string sumTotValVenta { get; set; } = "0.00";
    public string sumPrecioVenta { get; set; } = "0.00";
    public string sumDescTotal { get; set; } = "0.00";
    public string sumOtrosCargos { get; set; } = "0.00";
    public string sumTotalAnticipos { get; set; } = "0.00";
    public string sumImpVenta { get; set; } = "0.00";
    public string ublVersionId { get; set; } = "2.1";
    public string customizationId { get; set; } = "2.0";
}
