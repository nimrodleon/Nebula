using System.Text.Json.Serialization;

namespace Nebula.Modules.InvoiceHub.Dto;

public class AddressHub
{
    [JsonPropertyName("ubigueo")]
    public string Ubigueo { get; set; } = string.Empty;

    [JsonPropertyName("departamento")]
    public string Departamento { get; set; } = string.Empty;

    [JsonPropertyName("provincia")]
    public string Provincia { get; set; } = string.Empty;

    [JsonPropertyName("distrito")]
    public string Distrito { get; set; } = string.Empty;

    [JsonPropertyName("urbanizacion")]
    public string Urbanizacion { get; set; } = string.Empty;

    [JsonPropertyName("direccion")]
    public string Direccion { get; set; } = string.Empty;

    [JsonPropertyName("codLocal")]
    public string CodLocal { get; set; } = string.Empty;
}
