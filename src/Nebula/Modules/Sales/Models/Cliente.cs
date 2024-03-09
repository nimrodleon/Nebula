using Microsoft.EntityFrameworkCore;

namespace Nebula.Modules.Sales.Models;

[Owned]
public class Cliente
{
    public string TipoDoc { get; set; } = string.Empty;
    public string NumDoc { get; set; } = string.Empty;
    public string RznSocial { get; set; } = string.Empty;
}
