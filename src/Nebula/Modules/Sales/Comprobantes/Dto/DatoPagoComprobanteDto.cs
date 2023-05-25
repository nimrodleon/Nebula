namespace Nebula.Modules.Sales.Comprobantes.Dto;

public class DatoPagoComprobanteDto
{
    public string FormaPago { get; set; } = "Contado";
    public decimal MtoNetoPendientePago { get; set; } = 0;
}
