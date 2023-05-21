using Nebula.Modules.Lycet.Models.Company;

namespace Nebula.Modules.Lycet.Models.Sale;

public class LycetInvoice : LycetBaseSale
{
    /// <summary>
    /// Tipo operacion (Catálogo 51).
    /// </summary>
    public string TipoOperacion { get; set; } = string.Empty;

    public DateTime FecVencimiento { get; set; }
    public decimal SumDsctoGlobal { get; set; }
    public decimal MtoDescuentos { get; set; }
    public decimal SumOtrosDescuentos { get; set; }
    public List<LycetCharge> Descuentos { get; set; } = new List<LycetCharge>();
    public List<LycetCharge> Cargos { get; set; } = new List<LycetCharge>();
    public decimal MtoCargos { get; set; }
    public decimal TotalAnticipos { get; set; }
    public LycetSalePerception Perception { get; set; } = new LycetSalePerception();
    /// <summary>
    /// Utilizado cuando se trata de una Factura Guia.
    /// </summary>
    public LycetEmbededDespatch GuiaEmbebida { get; set; } = new LycetEmbededDespatch();
    public List<LycetPrepayment> Anticipos { get; set; } = new List<LycetPrepayment>();
    public LycetDetraction Detraccion { get; set; } = new LycetDetraction();
    public LycetClient Seller { get; set; } = new LycetClient();

    public decimal ValorVenta { get; set; }
    public decimal SubTotal { get; set; }
    public string Observacion { get; set; } = string.Empty;
    public LycetAddress DireccionEntrega { get; set; } = new LycetAddress();
}
