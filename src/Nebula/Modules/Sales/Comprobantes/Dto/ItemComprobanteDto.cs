using Nebula.Modules.Account.Models;
using Nebula.Modules.Sales.Helpers;

namespace Nebula.Modules.Sales.Comprobantes.Dto;

public class ItemComprobanteDto
{
    public string TipoItem { get; set; } = "BIEN";
    public decimal CtdUnidadItem { get; set; } = 0;
    public string CodUnidadMedida { get; set; } = "NIU:UNIDAD (BIENES)";
    public string Description { get; set; } = string.Empty;
    public string IgvSunat { get; set; } = TipoIGV.Gravado;
    #region INVENTORY_CONFIGURATION!
    public string ProductId { get; set; } = "-";
    public decimal MtoPrecioVentaUnitario { get; set; } = 0;
    public string WarehouseId { get; set; } = "-";
    #endregion
    public string RecordType { get; set; } = "PRODUCTO";

    private decimal GetPorcentajeIgv(Company company)
    {
        return IgvSunat == TipoIGV.Gravado ? (company.PorcentajeIgv / 100) + 1 : 1;
    }

    private decimal GetTotalVenta()
    {
        return CtdUnidadItem * MtoPrecioVentaUnitario;
    }

    public decimal GetMtoValorUnitario(Company company)
    {
        return MtoPrecioVentaUnitario / GetPorcentajeIgv(company);
    }

    public decimal GetMtoValorVenta(Company company)
    {
        return GetTotalVenta() / GetPorcentajeIgv(company);
    }

    public decimal GetMtoBaseIgv(Company company)
    {
        return GetTotalVenta() / GetPorcentajeIgv(company);
    }

    public decimal GetIgv(Company company)
    {
        return GetTotalVenta() - GetMtoValorVenta(company);
    }

}
