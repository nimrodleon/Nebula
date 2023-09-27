namespace Nebula.Modules.InvoiceHub.Dto;

public class InvoiceRequestHub
{
    public string Ruc { get; set; } = string.Empty;
    public string TipoOperacion { get; set; } = string.Empty;
    public string TipoDoc { get; set; } = string.Empty;
    public string Serie { get; set; } = string.Empty;
    public string Correlativo { get; set; } = string.Empty;
    public FormaPagoHub FormaPago { get; set; } = new FormaPagoHub();
    public string TipoMoneda { get; set; } = string.Empty;
    public ClientHub Client { get; set; } = new ClientHub();
    public List<DetailHub> Details { get; set; } = new List<DetailHub>();
}
