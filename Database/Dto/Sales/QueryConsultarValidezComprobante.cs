namespace Nebula.Database.Dto.Sales;

public static class TypeConsultarValidez
{
    public const string Dia = "DIA";
    public const string Mensual = "MENSUAL";
}

public class QueryConsultarValidezComprobante
{
    public string Type { get; set; } = TypeConsultarValidez.Dia;
    public string Date { get; set; } = string.Empty;
    public string Month { get; set; } = string.Empty;
    public string Year { get; set; } = string.Empty;
}
