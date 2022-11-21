namespace Nebula.Database.ViewModels.Sales;

public class CabeceraComprobanteDto
{
    public string ModoEnvio { get; set; } = "FIRMAR";
    public string TipDocUsuario { get; set; } = string.Empty;
    public string NumDocUsuario { get; set; } = string.Empty;
    public string RznSocialUsuario { get; set; } = string.Empty;
    public string FecVencimiento { get; set; } = "-";
}
