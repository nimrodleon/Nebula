using Nebula.Common.Dto;

namespace Nebula.Modules.Contacts.Dto;

public class ContactSelect : InputSelect2
{
    public string CompanyId { get; set; } = string.Empty;
    public string Document { get; set; } = string.Empty;
    public string DocType { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string CodUbigeo { get; set; } = string.Empty;
}
