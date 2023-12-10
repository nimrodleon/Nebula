using Nebula.Modules.Inventory.Models;

namespace Nebula.Modules.Inventory.Dto;

public class InventoryNoteDto
{
    public InventoryNotas InventoryNotas { get; set; } = new InventoryNotas();
    public List<InventoryNotasDetail> InventoryNotasDetail { get; set; } = new List<InventoryNotasDetail>();
}
