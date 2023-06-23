using Nebula.Modules.Purchases.Models;

namespace Nebula.Modules.Purchases.Dto;

public class CabeceraCompraDto
{
    public string DocType { get; set; } = "BOLETA";
    public string ContactId { get; set; } = string.Empty;
    public string TipDocProveedor { get; set; } = string.Empty;
    public string NumDocProveedor { get; set; } = string.Empty;
    public string RznSocialProveedor { get; set; } = string.Empty;
    public string FecEmision { get; set; } = string.Empty;
    public string SerieComprobante { get; set; } = string.Empty;
    public string NumComprobante { get; set; } = string.Empty;
    public string TipoMoneda { get; set; } = string.Empty;
    public decimal TipoDeCambio { get; set; } = 1M;
    // public string? FecVencimiento { get; set; } = "-";
    #region DIRECCIÓN_DEL_PROVEEDOR!
    public string CodUbigeoProveedor { get; set; } = "-";
    public string DesDireccionProveedor { get; set; } = string.Empty;
    #endregion

    public PurchaseInvoice GetPurchaseInvoice()
    {
        var fecha = DateTime.Parse(FecEmision);
        return new PurchaseInvoice()
        {
            Id = string.Empty,
            FecEmision = FecEmision,
            DocType = DocType,
            Serie = SerieComprobante.Trim(),
            Number = NumComprobante.Trim(),
            ContactId = ContactId,
            TipDocProveedor = TipDocProveedor.Trim(),
            NumDocProveedor = NumDocProveedor.Trim(),
            RznSocialProveedor = RznSocialProveedor.Trim(),
            TipMoneda = TipoMoneda,
            TipoCambio = TipoDeCambio,
            // calculo de importes.
            SumTotTributos = 0M,
            SumTotValCompra = 0M,
            SumPrecioCompra = 0M,
            SumImpCompra = 0M,
            Year = fecha.Year.ToString(),
            Month = fecha.Month.ToString("D2"),
        };
    }
}
