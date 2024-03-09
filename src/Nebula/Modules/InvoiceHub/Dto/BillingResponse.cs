using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace Nebula.Modules.InvoiceHub.Dto;

[Owned]
public class BillingResponse
{
    [JsonPropertyName("success")]
    public bool Success { get; set; } = false;

    [JsonPropertyName("hash")]
    public string Hash { get; set; } = string.Empty;

    [JsonPropertyName("cdrCode")]
    public string CdrCode { get; set; } = string.Empty;

    [JsonPropertyName("cdrDescription")]
    public string CdrDescription { get; set; } = string.Empty;

    [JsonPropertyName("cdrNotes")]
    public List<string> CdrNotes { get; set; } = new List<string>();

    [JsonPropertyName("cdrId")]
    public string CdrId { get; set; } = string.Empty;
}
