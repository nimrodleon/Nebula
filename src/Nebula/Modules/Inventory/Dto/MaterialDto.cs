using Nebula.Modules.Inventory.Models;

namespace Nebula.Modules.Inventory.Dto;

public class MaterialDto
{
    public Material Material { get; set; } = new Material();
    public List<MaterialDetail> MaterialDetails { get; set; } = new List<MaterialDetail>();
}
