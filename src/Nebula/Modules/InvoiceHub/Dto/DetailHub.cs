using System.Text.Json.Serialization;

namespace Nebula.Modules.InvoiceHub.Dto;

public class DetailHub
{
    [JsonPropertyName("codProducto")]
    public string CodProducto { get; set; } = string.Empty;

    [JsonPropertyName("unidad")]
    public string Unidad { get; set; } = string.Empty;

    [JsonPropertyName("cantidad")]
    public decimal Cantidad { get; set; }

    [JsonPropertyName("mtoValorUnitario")]
    public decimal MtoValorUnitario { get; set; }

    [JsonPropertyName("descripcion")]
    public string Descripcion { get; set; } = string.Empty;

    [JsonPropertyName("mtoBaseIgv")]
    public decimal MtoBaseIgv { get; set; }

    [JsonPropertyName("porcentajeIgv")]
    public decimal PorcentajeIgv { get; set; }

    [JsonPropertyName("igv")]
    public decimal Igv { get; set; }

    [JsonPropertyName("tipAfeIgv")]
    public string TipAfeIgv { get; set; } = string.Empty;

    [JsonPropertyName("totalImpuestos")]
    public decimal TotalImpuestos { get; set; }

    [JsonPropertyName("mtoValorVenta")]
    public decimal MtoValorVenta { get; set; }

    [JsonPropertyName("mtoPrecioUnitario")]
    public decimal MtoPrecioUnitario { get; set; }
}
