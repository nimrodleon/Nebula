using System.Text.Json;

namespace Nebula.Database.Services.Facturador;

public class JsonBoletaParser
{
    public Invoice cabecera { get; set; } = new Invoice();
    public List<InvoiceDetail> detalle { get; set; } = new List<InvoiceDetail>();
    public List<Tributo> tributos { get; set; } = new List<Tributo>();
    public List<Leyenda> leyendas { get; set; } = new List<Leyenda>();

    public void CreateJson(string path)
    {
        string jsonString = JsonSerializer.Serialize(this);
        File.WriteAllText(path, jsonString);
    }
}
