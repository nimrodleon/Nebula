using System.Text.Json.Serialization;

namespace Nebula.Modules.InvoiceHub.Dto;

public class FormaPagoHub
{
    [JsonPropertyName("moneda")]
    public string Moneda { get; set; } = string.Empty;

    [JsonPropertyName("tipo")]
    public string Tipo { get; set; } = string.Empty;

    [JsonPropertyName("monto")]
    public decimal Monto { get; set; }
}
