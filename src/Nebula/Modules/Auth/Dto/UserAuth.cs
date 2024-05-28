using Nebula.Modules.Account.Models;
using Nebula.Modules.Auth.Models;

namespace Nebula.Modules.Auth.Dto;

public class UserAuth
{
    public User User { get; set; } = new User();
    public List<Company> Companies { get; set; } = new List<Company>();
}
