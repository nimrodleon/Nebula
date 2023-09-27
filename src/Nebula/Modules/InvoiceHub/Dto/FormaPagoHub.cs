namespace Nebula.Modules.InvoiceHub.Dto;

public class FormaPagoHub
{
    public string Moneda { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty;
    public decimal Monto { get; set; }
}
