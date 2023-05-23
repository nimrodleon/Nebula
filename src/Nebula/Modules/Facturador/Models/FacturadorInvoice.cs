namespace Nebula.Modules.Facturador.Models;

public class FacturadorInvoice
{
    public string tipOperacion { get; set; } = string.Empty;
    public string fecEmision { get; set; } = string.Empty;
    public string horEmision { get; set; } = string.Empty;
    public string? fecVencimiento { get; set; } = null;
    public string? codLocalEmisor { get; set; } = null;
    public string tipDocUsuario { get; set; } = string.Empty;
    public string numDocUsuario { get; set; } = string.Empty;
    public string rznSocialUsuario { get; set; } = string.Empty;
    public string tipMoneda { get; set; } = string.Empty;
    public string sumTotTributos { get; set; } = string.Empty;
    public string sumTotValVenta { get; set; } = string.Empty;
    public string sumPrecioVenta { get; set; } = "0.00";
    public string sumDescTotal { get; set; } = "0.00";
    public string sumOtrosCargos { get; set; } = "0.00";
    public string sumTotalAnticipos { get; set; } = "0.00";
    public string sumImpVenta { get; set; } = string.Empty;
    public string ublVersionId { get; set; } = "2.1";
    public string customizationId { get; set; } = "2.0";
    public FacturadorAdicionalCabecera? adicionalCabecera { get; set; } = null;
}
