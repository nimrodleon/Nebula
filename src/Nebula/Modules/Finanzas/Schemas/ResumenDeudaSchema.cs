namespace Nebula.Modules.Finanzas.Schemas;

public class ResumenDeudaSchema
{
    public decimal TotalPorCobrar { get; set; }
    public List<DeudaClienteSchema> DeudasPorCliente { get; set; } = new List<DeudaClienteSchema>();
}
