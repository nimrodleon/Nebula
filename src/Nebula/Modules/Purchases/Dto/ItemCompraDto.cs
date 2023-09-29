using Nebula.Modules.Account.Models;
using Nebula.Modules.Purchases.Helpers;
using Nebula.Modules.Purchases.Models;
using Nebula.Modules.Sales.Helpers;

namespace Nebula.Modules.Purchases.Dto;

public class ItemCompraDto
{
    public string? Id { get; set; } = null;
    public string ProductId { get; set; } = "-";
    public string TipoItem { get; set; } = "BIEN";
    public decimal CtdUnidadItem { get; set; } = 0;
    public string CodUnidadMedida { get; set; } = "NIU:UNIDAD (BIENES)";
    public string DesItem { get; set; } = string.Empty;
    public bool TriIcbper { get; set; } = false;
    public string IgvSunat { get; set; } = TipoIGV.Gravado;
    public decimal MtoPrecioCompraUnitario { get; set; } = 0;
    //#region CONTROL_LOTE_PRODUCCIÓN
    //public bool hasLotes { get; set; } = false;
    //public string productLoteId { get; set; } = string.Empty;
    //#endregion

    public PurchaseInvoiceDetail GetDetail(Company company, string purchaseInvoiceId)
    {
        // Tributo: Afectación al IGV por ítem.
        string tipAfeIgv = "10";
        string codTriIgv = string.Empty;
        string nomTributoIgvItem = string.Empty;
        string codTipTributoIgvItem = string.Empty;
        switch (IgvSunat)
        {
            case TipoIGV.Gravado:
                tipAfeIgv = "10";
                codTriIgv = "1000";
                nomTributoIgvItem = "IGV";
                codTipTributoIgvItem = "VAT";
                break;
            case TipoIGV.Exonerado:
                tipAfeIgv = "20";
                codTriIgv = "9997";
                nomTributoIgvItem = "EXO";
                codTipTributoIgvItem = "VAT";
                break;
            case TipoIGV.Inafecto:
                tipAfeIgv = "30";
                codTriIgv = "9998";
                nomTributoIgvItem = "INA";
                codTipTributoIgvItem = "FRE";
                break;
        }
        var itemImporte = new ItemImporteCompra(company);
        itemImporte = itemImporte.CalcularImporte(this);
        return new PurchaseInvoiceDetail()
        {
            Id = Id != null ? Id : string.Empty,
            PurchaseInvoiceId = purchaseInvoiceId,
            TipoItem = TipoItem,
            CodUnidadMedida = CodUnidadMedida,
            CtdUnidadItem = CtdUnidadItem,
            CodProducto = ProductId,
            DesItem = DesItem.Trim(),
            MtoValorUnitario = itemImporte.MtoValorUnitario,
            SumTotTributosItem = itemImporte.SumTotTributosItem,
            // Tributo: IGV(1000).
            CodTriIgv = codTriIgv,
            MtoIgvItem = itemImporte.MtoIgvItem,
            MtoBaseIgvItem = itemImporte.MtoBaseIgvItem,
            NomTributoIgvItem = nomTributoIgvItem,
            CodTipTributoIgvItem = codTipTributoIgvItem,
            TipAfeIgv = tipAfeIgv,
            PorIgvItem = IgvSunat == TipoIGV.Gravado ? company.PorcentajeIgv : 0,
            // Tributo ICBPER 7152.
            CodTriIcbper = TriIcbper ? "7152" : "-",
            MtoTriIcbperItem = TriIcbper ? itemImporte.MtoTriIcbperItem : 0,
            CtdBolsasTriIcbperItem = TriIcbper ? Convert.ToInt32(CtdUnidadItem) : 0,
            NomTributoIcbperItem = "ICBPER",
            CodTipTributoIcbperItem = "OTH",
            MtoTriIcbperUnidad = TriIcbper ? company.ValorImpuestoBolsa : 0,
            // Precio de Compra Unitario.
            MtoPrecioCompraUnitario = MtoPrecioCompraUnitario,
            MtoValorCompraItem = itemImporte.MtoValorCompraItem,
        };
    }
}
