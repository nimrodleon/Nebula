namespace Nebula.Database.Dto.Common;

public class ContactSelect : InputSelect2
{
    public string DocType { get; set; } = string.Empty;
    public string Document { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}
