using Nebula.Modules.Account.Models;
using Nebula.Modules.Cashier.Models;
using Nebula.Modules.Contacts.Models;

namespace Nebula.Modules.Cashier.Dto;

public class QuickSaleConfig
{
    public Company Company { get; set; } = new Company();
    public CajaDiaria CajaDiaria { get; set; } = new CajaDiaria();
    public Contact Contact { get; set; } = new Contact();
}
