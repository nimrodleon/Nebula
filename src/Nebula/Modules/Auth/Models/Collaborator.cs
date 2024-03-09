using Nebula.Modules.Auth.Helpers;
using System.ComponentModel.DataAnnotations;
using Nebula.Modules.Account.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nebula.Modules.Auth.Models;

public class Collaborator
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid? CompanyId { get; set; } = null;

    [ForeignKey(nameof(CompanyId))]
    public Company Company { get; set; } = new Company();

    public Guid? UserId { get; set; } = null;

    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = new User();

    public string UserRole { get; set; } = CompanyRoles.User;
}
