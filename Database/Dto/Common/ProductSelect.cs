using Nebula.Database.Helpers;

namespace Nebula.Database.Dto.Common;

public class ProductSelect : InputSelect2
{
    public string Description { get; set; } = string.Empty;
    public string Barcode { get; set; } = "-";
    public string CodProductoSUNAT { get; set; } = "-";
    public decimal PrecioVentaUnitario { get; set; }
    public string IgvSunat { get; set; } = TipoIGV.Gravado;
    public string Icbper { get; set; } = "NO";
    public string ControlStock { get; set; } = "NONE";
}
