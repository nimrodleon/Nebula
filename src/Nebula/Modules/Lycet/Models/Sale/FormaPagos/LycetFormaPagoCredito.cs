namespace Nebula.Modules.Lycet.Models.Sale.FormaPagos;

public class LycetFormaPagoCredito : LycetPaymentTerms
{
    public LycetFormaPagoCredito(decimal? monto = null, string? moneda = null)
    {
        Tipo = "Credito";
        Monto = monto;
        Moneda = moneda;
    }
}
