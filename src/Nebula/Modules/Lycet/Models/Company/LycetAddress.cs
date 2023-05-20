namespace Nebula.Modules.Lycet.Models.Company;

public class LycetAddress
{
    public string? Ubigueo { get; set; }
    public string? CodigoPais { get; set; } = "PE";
    public string? Departamento { get; set; }
    public string? Provincia { get; set; }
    public string? Distrito { get; set; }
    public string? Urbanizacion { get; set; }
    public string? Direccion { get; set; }
    public string? CodLocal { get; set; } = "0000";
}
