namespace Nebula.Database.Dto.Sales;

public class DatoPagoComprobanteDto
{
    public string FormaPago { get; set; } = "Contado";
    public decimal MtoNetoPendientePago { get; set; } = 0;
}
