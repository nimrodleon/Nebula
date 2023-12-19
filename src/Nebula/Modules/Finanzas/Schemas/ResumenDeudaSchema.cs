namespace Nebula.Modules.Finanzas.Schemas;

public class ResumenDeudaSchema
{
    public decimal TotalPorCobrar { get; set; }
    public List<ClienteDeudaSchema> DeudasPorCliente { get; set; } = new List<ClienteDeudaSchema>();
}
