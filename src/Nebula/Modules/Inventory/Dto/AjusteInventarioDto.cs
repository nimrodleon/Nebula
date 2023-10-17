using Nebula.Modules.Inventory.Models;

namespace Nebula.Modules.Inventory.Dto;

public class AjusteInventarioDto
{
    public AjusteInventario AjusteInventario { get; set; } = new AjusteInventario();
    public List<AjusteInventarioDetail> AjusteInventarioDetails { get; set; } = new List<AjusteInventarioDetail>();
}
