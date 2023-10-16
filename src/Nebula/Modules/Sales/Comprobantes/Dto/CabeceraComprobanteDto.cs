namespace Nebula.Modules.Sales.Comprobantes.Dto;

public class CabeceraComprobanteDto
{
    public string CajaDiariaId { get; set; } = "-";
    public string TipoDoc { get; set; } = "03";
    public string ContactId { get; set; } = string.Empty;
    public string TipDocUsuario { get; set; } = string.Empty;
    public string NumDocUsuario { get; set; } = string.Empty;
    public string RznSocialUsuario { get; set; } = string.Empty;
    public string PaymentMethod { get; set; } = string.Empty;
    public string FecVencimiento { get; set; } = "-";
    public string InvoiceSerieId { get; set; } = "-";
    public string Remark { get; set; } = string.Empty;
}
