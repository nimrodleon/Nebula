namespace Nebula.Modules.Lycet.Models.Sale;

public class LycetPrepayment
{
    public string TipoDocRel { get; set; } = string.Empty;
    public string NroDocRel { get; set; } = string.Empty;
    public decimal Total { get; set; }
}
