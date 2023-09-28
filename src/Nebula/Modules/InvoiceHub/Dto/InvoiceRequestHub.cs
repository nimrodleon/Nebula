using System.Text.Json.Serialization;

namespace Nebula.Modules.InvoiceHub.Dto;

public class InvoiceRequestHub
{
    [JsonPropertyName("ruc")]
    public string Ruc { get; set; } = string.Empty;

    [JsonPropertyName("tipoOperacion")]
    public string TipoOperacion { get; set; } = string.Empty;

    [JsonPropertyName("tipoDoc")]
    public string TipoDoc { get; set; } = string.Empty;

    [JsonPropertyName("serie")]
    public string Serie { get; set; } = string.Empty;

    [JsonPropertyName("correlativo")]
    public string Correlativo { get; set; } = string.Empty;

    [JsonPropertyName("formaPago")]
    public FormaPagoHub FormaPago { get; set; } = new FormaPagoHub();

    [JsonPropertyName("tipoMoneda")]
    public string TipoMoneda { get; set; } = string.Empty;

    [JsonPropertyName("client")]
    public ClientHub Client { get; set; } = new ClientHub();

    [JsonPropertyName("details")]
    public List<DetailHub> Details { get; set; } = new List<DetailHub>();
}
