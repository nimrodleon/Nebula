using System.Text.Json.Serialization;

namespace Nebula.Modules.InvoiceHub.Dto;

public class EmpresaHub
{
    [JsonPropertyName("ruc")]
    public string Ruc { get; set; } = string.Empty;

    [JsonPropertyName("companyId")]
    public string CompanyId { get; set; } = string.Empty;

    [JsonPropertyName("razonSocial")]
    public string RazonSocial { get; set; } = string.Empty;

    [JsonPropertyName("nombreComercial")]
    public string NombreComercial { get; set; } = string.Empty;

    [JsonPropertyName("sunatEndpoint")]
    public string SunatEndpoint { get; set; } = string.Empty;

    [JsonPropertyName("claveSol")]
    public ClaveSolHub ClaveSol { get; set; } = new ClaveSolHub();

    [JsonPropertyName("address")]
    public AddressHub Address { get; set; } = new AddressHub();
}
