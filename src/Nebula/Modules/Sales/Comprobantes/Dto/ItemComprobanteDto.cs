using Nebula.Modules.Sales.Helpers;

namespace Nebula.Modules.Sales.Comprobantes.Dto;

public class ItemComprobanteDto
{
    public string TipoItem { get; set; } = "BIEN";
    public decimal CtdUnidadItem { get; set; } = 0;
    public string CodUnidadMedida { get; set; } = "NIU:UNIDAD (BIENES)";
    public string DesItem { get; set; } = string.Empty;
    public bool TriIcbper { get; set; } = false;
    public string IgvSunat { get; set; } = TipoIGV.Gravado;
    #region INVENTORY_CONFIGURATION!
    public string ProductId { get; set; } = "-";
    public decimal MtoPrecioVentaUnitario { get; set; } = 0;
    public string WarehouseId { get; set; } = "-";
    #endregion
}
