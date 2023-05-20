namespace Nebula.Modules.Lycet.Models.Company;

public class LycetCompany
{
    public string? Ruc { get; set; }
    public string? RazonSocial { get; set; }
    public string? NombreComercial { get; set; }
    public LycetAddress? Address { get; set; }
    public string? Email { get; set; }
    public string? Telephone { get; set; }
}
