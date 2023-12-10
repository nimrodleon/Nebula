using System.Text.Json.Serialization;

namespace Nebula.Modules.InvoiceHub.Dto;

public class CreditNoteRequestHub
{
    [JsonPropertyName("ruc")]
    public string Ruc { get; set; } = string.Empty;

    [JsonPropertyName("serie")]
    public string Serie { get; set; } = string.Empty;

    [JsonPropertyName("correlativo")]
    public string Correlativo { get; set; } = string.Empty;

    [JsonPropertyName("fechaEmision")]
    public string FechaEmision { get; set; } = string.Empty;

    [JsonPropertyName("tipDocAfectado")]
    public string TipDocAfectado { get; set; } = string.Empty;

    [JsonPropertyName("numDocAfectado")]
    public string NumDocAfectado { get; set; } = string.Empty;

    [JsonPropertyName("codMotivo")]
    public string CodMotivo { get; set; } = string.Empty;

    [JsonPropertyName("desMotivo")]
    public string DesMotivo { get; set; } = string.Empty;

    [JsonPropertyName("tipoMoneda")]
    public string TipoMoneda { get; set; } = string.Empty;

    [JsonPropertyName("client")]
    public ClientHub Client { get; set; } = new ClientHub();

    [JsonPropertyName("details")]
    public List<DetailHub> Details { get; set; } = new List<DetailHub>();
}
