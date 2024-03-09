using Nebula.Modules.Auth.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nebula.Modules.Account.Models;

public class PagoSuscripcion
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid? UserId { get; set; } = null;

    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = new User();

    public Guid? CompanyId { get; set; } = null;

    [ForeignKey(nameof(CompanyId))]
    public Company Company { get; set; } = new Company();

    public string FechaDesde { get; set; } = string.Empty;
    public string FechaHasta { get; set; } = string.Empty;
    public decimal Monto { get; set; } = decimal.Zero;
    public string Remark { get; set; } = string.Empty;
    public string CreatedAt { get; set; } = DateTime.Now.ToString("yyyy-MM-dd");
    public string Year { get; set; } = DateTime.Now.ToString("yyyy");
    public string Month { get; set; } = DateTime.Now.ToString("MM");
}
