using Nebula.Modules.Sales.Helpers;

namespace Nebula.Modules.Purchases.Dto;

public class PurchaseDetailDto
{
    public string TipoItem { get; set; } = "BIEN";
    public decimal CtdUnidadItem { get; set; } = 0;
    public string CodUnidadMedida { get; set; } = "NIU:UNIDAD (BIENES)";
    public string DesItem { get; set; } = string.Empty;
    public bool TriIcbper { get; set; } = false;
    public string IgvSunat { get; set; } = TipoIGV.Gravado;
    #region INVENTORY_CONFIGURATION!
    public string ProductId { get; set; } = "-";
    public decimal MtoPrecioCompraUnitario { get; set; } = 0;
    #endregion
    //#region CONTROL_LOTE_PRODUCCIÓN
    //public bool hasLotes { get; set; } = false;
    //public string productLoteId { get; set; } = string.Empty;
    //#endregion
}
