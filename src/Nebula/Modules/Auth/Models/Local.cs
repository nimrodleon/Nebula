using System.ComponentModel.DataAnnotations;

namespace Nebula.Modules.Auth.Models;

public class Local
{
    public int Id { get; set; }

    [MaxLength(100)]
    public string ShortName { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Address { get; set; } = string.Empty;

    [MaxLength(100)]
    public string CodigoSunat { get; set; } = string.Empty;
}
