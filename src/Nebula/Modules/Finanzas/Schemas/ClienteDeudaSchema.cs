using Nebula.Modules.Finanzas.Models;

namespace Nebula.Modules.Finanzas.Schemas;

public class ClienteDeudaSchema
{
    public string ContactId { get; set; } = string.Empty;
    public string ContactName { get; set; } = string.Empty;
    public decimal DeudaTotal { get; set; }
    public List<AccountsReceivable> Receivables { get; set; } = new List<AccountsReceivable>();
}
