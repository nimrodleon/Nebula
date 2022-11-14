using System.Text.Json;

namespace Nebula.Database.Services.Facturador;

public class JsonFacturaParser
{
    public Invoice cabecera { get; set; } = new Invoice();
    public List<InvoiceDetail> detalle { get; set; } = new List<InvoiceDetail>();
    public List<Tributo> tributos { get; set; } = new List<Tributo>();
    public List<Leyenda> leyendas { get; set; } = new List<Leyenda>();
    public FormaPago datoPago { get; set; } = new FormaPago();
    public List<ItemFormaPago> detallePago { get; set; } = new List<ItemFormaPago>();

    public void CreateJson(string path)
    {
        string jsonString = JsonSerializer.Serialize(this);
        File.WriteAllText(path, jsonString);
    }
}
