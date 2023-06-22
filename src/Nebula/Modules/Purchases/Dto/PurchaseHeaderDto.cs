namespace Nebula.Modules.Purchases.Dto;

public class PurchaseHeaderDto
{
    public string DocType { get; set; } = "BOLETA";
    public string ContactId { get; set; } = string.Empty;
    public string TipDocProveedor { get; set; } = string.Empty;
    public string NumDocProveedor { get; set; } = string.Empty;
    public string RznSocialProveedor { get; set; } = string.Empty;
    public string FecEmision { get; set; } = string.Empty;
    public string NumComprobante { get; set; } = string.Empty;
    public decimal TipoDeCambio { get; set; } = 1M;
    public string? FecVencimiento { get; set; } = "-";
    #region DIRECCIÓN_DEL_PROVEEDOR!
    public string CodUbigeoProveedor { get; set; } = "-";
    public string DesDireccionProveedor { get; set; } = string.Empty;
    #endregion
}
