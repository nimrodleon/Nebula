namespace Nebula.Database.Dto.Common;

public class ProductSelect : InputSelect2
{
    public string Description { get; set; } = string.Empty;
    public string Barcode { get; set; } = string.Empty;
    public decimal Price { get; set; }
}
