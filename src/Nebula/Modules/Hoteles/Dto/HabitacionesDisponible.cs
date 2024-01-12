namespace Nebula.Modules.Hoteles.Dto;

public class HabitacionDisponible
{
    public string HabitacionId { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Piso { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public decimal Precio { get; set; }
}
