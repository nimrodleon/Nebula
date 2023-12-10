using System.Text.Json.Serialization;

namespace Nebula.Modules.InvoiceHub.Dto;

public class CuotaHub
{
    [JsonPropertyName("moneda")]
    public string Moneda { get; set; } = string.Empty;

    [JsonPropertyName("monto")]
    public decimal Monto { get; set; }

    [JsonPropertyName("fechaPago")]
    public string FechaPago { get; set; } = string.Empty;
}
