using Nebula.Modules.Lycet.Models.Company;

namespace Nebula.Modules.Lycet.Models.Sale;

public class LycetBaseSale
{
    protected string UblVersion { get; set; } = "2.0";
    protected string TipoDoc { get; set; } = string.Empty;
    protected string Serie { get; set; } = string.Empty;
    protected string Correlativo { get; set;} = string.Empty;
    protected DateTime FechaEmision { get; set; }
    protected LycetCompany Company { get; set; } = new LycetCompany();
    protected LycetClient Client { get; set; } = new LycetClient();
    protected string TipoMoneda { get; set; } = string.Empty;
    protected decimal SumOtrosCargos { get; set; }
    protected decimal MtoOperGravadas { get; set; }
    protected decimal MtoOperInafectas { get; set; }
    protected decimal MtoOperExoneradas { get; set; }
    protected decimal MtoOperExportacion { get; set; }
    protected decimal MtoOperGratuitas { get; set; }
    protected decimal MtoIGVGratuitas { get; set; }
    protected decimal MtoIGV { get; set; }
    protected decimal MtoBaseIvap { get; set; }
    protected decimal MtoIvap { get; set; }
    protected decimal MtoBaseIsc { get; set; }
    protected decimal MtoISC { get; set; }
    protected decimal MtoBaseOth { get; set; }
    protected decimal MtoOtrosTributos { get; set; }
    protected decimal Icbper { get; set; }
    protected decimal TotalImpuestos { get; set; }
    protected decimal Redondeo { get; set; }
    /// <summary>
    /// Importe total de la venta, cesión en uso o del servicio prestado.
    /// </summary>
    protected decimal MtoImpVenta { get; set; }

    protected List<LycetSaleDetail> Details { get; set; } = new List<LycetSaleDetail>();

}
