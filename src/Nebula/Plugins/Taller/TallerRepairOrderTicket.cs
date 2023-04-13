using Nebula.Plugins.Taller.Models;

namespace Nebula.Plugins.Taller;

/// <summary>
/// Modelo de datos para Imprimir un Ticket.
/// </summary>
public class TallerRepairOrderTicket
{
    public TallerRepairOrder RepairOrder { get; set; } = new TallerRepairOrder();
}
