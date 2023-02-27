namespace Nebula.Database.Services.Facturador;

public class InvoiceDetail
{
    public string codUnidadMedida { get; set; } = string.Empty;
    public string ctdUnidadItem { get; set; } = string.Empty;
    public string? codProducto { get; set; } = null;
    public string? codProductoSUNAT { get; set; } = null;
    public string desItem { get; set; } = string.Empty;
    public string mtoValorUnitario { get; set; } = string.Empty;
    public string sumTotTributosItem { get; set; } = string.Empty;
    #region Tributo: IGV(1000)
    public string codTriIGV { get; set; } = "1000";
    public string mtoIgvItem { get; set; } = "0.00";
    public string mtoBaseIgvItem { get; set; } = "0.00";
    public string nomTributoIgvItem { get; set; } = "IGV";
    public string codTipTributoIgvItem { get; set; } = "VAT";
    public string tipAfeIGV { get; set; } = string.Empty;
    public string porIgvItem { get; set; } = string.Empty;
    #endregion
    public string codTriISC { get; set; } = "-";
    public string codTriOtro { get; set; } = "-";
    #region Tributo ICBPER 7152
    public string codTriIcbper { get; set; } = "-";
    public string mtoTriIcbperItem { get; set; } = "0.00";
    public string ctdBolsasTriIcbperItem { get; set; } = "0";
    public string nomTributoIcbperItem { get; set; } = "ICBPER";
    public string codTipTributoIcbperItem { get; set; } = "OTH";
    public string mtoTriIcbperUnidad { get; set; } = "0.00";
    #endregion
    public string mtoPrecioVentaUnitario { get; set; } = string.Empty;
    public string mtoValorVentaItem { get; set; } = string.Empty;
    public string mtoValorReferencialUnitario { get; set; } = "0.00";
}
