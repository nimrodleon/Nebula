using Nebula.Modules.Configurations.Models;
using Nebula.Modules.Purchases.Models;
using Nebula.Modules.Sales.Helpers;

namespace Nebula.Modules.Purchases.Dto;

public class PurchaseDataDto
{
    private Configuration _configuration = new Configuration();
    #region ORIGIN_HTTP_REQUEST!
    public PurchaseHeaderDto HeaderDto { get; set; } = new PurchaseHeaderDto();
    public List<PurchaseDetailDto> DetailDtos { get; set; } = new List<PurchaseDetailDto>();
    #endregion
    public List<PurchaseImporteItemDto> ImporteItems { get; set; } = new List<PurchaseImporteItemDto>();
    public void SetConfiguration(Configuration configuration) => _configuration = configuration;

    public List<PurchaseInvoiceDetail> GetPurchaseInvoiceDetails(string purchaseInvoiceId)
    {
        var purchaseInvoiceDetails = new List<PurchaseInvoiceDetail>();

        DetailDtos.ForEach(item =>
        {
            // Tributo: Afectación al IGV por ítem.
            string tipAfeIgv = "10";
            string codTriIgv = string.Empty;
            string nomTributoIgvItem = string.Empty;
            string codTipTributoIgvItem = string.Empty;
            switch (item.IgvSunat)
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

            // calcular importes.
            PurchaseImporteItemDto impItemDto = new PurchaseImporteItemDto();
            impItemDto.IgvSunat = item.IgvSunat;
            decimal mtoTotalItem = item.CtdUnidadItem * item.MtoPrecioCompraUnitario;
            decimal porcentajeIGV = item.IgvSunat == TipoIGV.Gravado ? _configuration.PorcentajeIgv / 100 + 1 : 1;
            impItemDto.MtoValorCompraItem = mtoTotalItem / porcentajeIGV;
            impItemDto.MtoTriIcbperItem = item.TriIcbper ? item.CtdUnidadItem * _configuration.ValorImpuestoBolsa : 0;
            impItemDto.MtoBaseIgvItem = mtoTotalItem / porcentajeIGV;
            impItemDto.MtoIgvItem = mtoTotalItem - impItemDto.MtoBaseIgvItem;
            impItemDto.SumTotTributosItem = impItemDto.MtoIgvItem + impItemDto.MtoTriIcbperItem; // el sistema soporta solo IGV/ICBPER.
            impItemDto.MtoValorUnitario = impItemDto.MtoValorCompraItem / item.CtdUnidadItem;
            ImporteItems.Add(impItemDto);

            // agregar items al comprobante.
            purchaseInvoiceDetails.Add(new PurchaseInvoiceDetail()
            {
                PurchaseInvoiceId = purchaseInvoiceId,
                TipoItem = item.TipoItem,
                CodUnidadMedida = item.CodUnidadMedida,
                CtdUnidadItem = item.CtdUnidadItem,
                CodProducto = item.ProductId,
                DesItem = item.DesItem,
                MtoValorUnitario = impItemDto.MtoValorUnitario,
                SumTotTributosItem = impItemDto.SumTotTributosItem,
                // Tributo: IGV(1000).
                CodTriIgv = codTriIgv,
                MtoIgvItem = impItemDto.MtoIgvItem,
                MtoBaseIgvItem = impItemDto.MtoBaseIgvItem,
                NomTributoIgvItem = nomTributoIgvItem,
                CodTipTributoIgvItem = codTipTributoIgvItem,
                TipAfeIgv = tipAfeIgv,
                PorIgvItem = item.IgvSunat == TipoIGV.Gravado ? _configuration.PorcentajeIgv : 0,

            });
        });

        return purchaseInvoiceDetails;
    }
}
