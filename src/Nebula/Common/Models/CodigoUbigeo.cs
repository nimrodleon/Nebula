using System.ComponentModel.DataAnnotations;

namespace Nebula.Common.Models;

public class CodigoUbigeo
{
    [Key]
    public string IdUbigeo { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Departament { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Province { get; set; } = string.Empty;

    [MaxLength(100)]
    public string District { get; set; } = string.Empty;
}
