using Nebula.Modules.InvoiceHub.Dto;
using System.ComponentModel.DataAnnotations;
using Nebula.Modules.Account.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nebula.Modules.Sales.Models;

public abstract class BaseSale
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid? CompanyId { get; set; } = null;
    [ForeignKey(nameof(CompanyId))]
    public Company Company { get; set; } = new Company();
    public string InvoiceSerieId { get; set; } = string.Empty;
    public string TipoDoc { get; set; } = "03";
    public string Serie { get; set; } = string.Empty;
    public string Correlativo { get; set; } = string.Empty;
    public string FechaEmision { get; set; } = string.Empty;
    public string ContactId { get; set; } = string.Empty;
    public Cliente Cliente { get; set; } = new Cliente();
    public string TipoMoneda { get; set; } = "PEN";
    public decimal MtoOperGravadas { get; set; }
    public decimal MtoOperInafectas { get; set; }
    public decimal MtoOperExoneradas { get; set; }
    public decimal MtoIGV { get; set; }
    public decimal TotalImpuestos { get; set; }
    public decimal ValorVenta { get; set; }
    public decimal SubTotal { get; set; }
    public decimal MtoImpVenta { get; set; }
    public BillingResponse BillingResponse { get; set; } = new BillingResponse();
    public string TotalEnLetras { get; set; } = string.Empty;
    public string Year { get; set; } = DateTime.Now.ToString("yyyy");
    public string Month { get; set; } = DateTime.Now.ToString("MM");
}
