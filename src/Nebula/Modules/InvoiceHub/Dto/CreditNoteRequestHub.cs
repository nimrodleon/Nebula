namespace Nebula.Modules.InvoiceHub.Dto;

public class CreditNoteRequestHub
{
    public string Ruc { get; set; } = string.Empty;
    public string TipoDoc { get; set; } = string.Empty;
    public string Serie { get; set; } = string.Empty;
    public string Correlativo { get; set; } = string.Empty;
    public string TipDocAfectado { get; set; } = string.Empty;
    public string NumDocAfectado { get; set; } = string.Empty;
    public string CodMotivo { get; set; } = string.Empty;
    public string DesMotivo { get; set; } = string.Empty;
    public string TipoMoneda { get; set; } = string.Empty;
    public ClientHub Client { get; set; } = new ClientHub();
    public List<DetailHub> Details { get; set; } = new List<DetailHub>();
}
