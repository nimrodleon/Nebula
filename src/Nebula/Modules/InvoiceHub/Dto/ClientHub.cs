using System.Text.Json.Serialization;

namespace Nebula.Modules.InvoiceHub.Dto;

public class ClientHub
{
    [JsonPropertyName("tipoDoc")]
    public string TipoDoc { get; set; } = string.Empty;

    [JsonPropertyName("numDoc")]
    public string NumDoc { get; set; } = string.Empty;

    [JsonPropertyName("rznSocial")]
    public string RznSocial { get; set; } = string.Empty;
}
