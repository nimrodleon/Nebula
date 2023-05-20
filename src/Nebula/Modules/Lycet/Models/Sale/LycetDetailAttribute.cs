namespace Nebula.Modules.Lycet.Models.Sale;

public class LycetDetailAttribute
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public DateTime FecInicio { get; set; }
    public DateTime FecFin { get; set; }
    public int Duracion { get; set; }
}
