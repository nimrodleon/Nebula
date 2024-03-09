using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace Nebula.Modules.InvoiceHub.Dto;

[Owned]
public class ClaveSolHub
{
    [JsonPropertyName("user")]
    public string User { get; set; } = string.Empty;

    [JsonPropertyName("password")]
    public string Password { get; set; } = string.Empty;
}
