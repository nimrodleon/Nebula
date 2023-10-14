using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Nebula.Common.Models;
using Nebula.Modules.InvoiceHub.Dto;

namespace Nebula.Modules.Sales.Models;

[BsonIgnoreExtraElements]
public abstract class BaseSale : IGenericModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;
    public string CompanyId { get; set; } = string.Empty;
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
