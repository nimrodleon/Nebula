using Nebula.Modules.Inventory.Models;

namespace Nebula.Modules.Inventory.Dto;

public class TransferenciaDto
{
    public Transferencia Transferencia { get; set; } = new Transferencia();
    public List<TransferenciaDetail> TransferenciaDetails { get; set; } = new List<TransferenciaDetail>();
}
