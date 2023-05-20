namespace Nebula.Modules.Lycet.Models.Sale.FormaPagos;

public class LycetFormaPagoContado : LycetPaymentTerms
{
    public LycetFormaPagoContado()
    {
        Tipo = "Contado";
        Moneda = null;
        Monto = null;
    }
}
