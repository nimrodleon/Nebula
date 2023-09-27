namespace Nebula.Modules.InvoiceHub.Dto;

public class BillingResponse
{
    public bool Success { get; set; } = false;
    public string Hash { get; set; } = string.Empty;
    public string CdrCode { get; set; } = string.Empty;
    public string CdrDescription { get; set; } = string.Empty;
    public List<string> CdrNotes { get; set; } = new List<string>();
    public string CdrId { get; set; } = string.Empty;
}
