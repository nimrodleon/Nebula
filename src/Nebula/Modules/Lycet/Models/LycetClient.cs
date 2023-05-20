using Nebula.Modules.Lycet.Models.Company;

namespace Nebula.Modules.Lycet.Models;

public class LycetClient
{
    public string? TipoDoc { get; set; }
    public string? NumDoc { get; set; }
    public string? RznSocial { get; set; }
    public LycetAddress? Address { get; set; }
    public string? Email { get; set; }
    public string? Telephone { get; set; }
}
