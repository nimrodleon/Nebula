using Nebula.Modules.Auth.Models;
using Nebula.Modules.Configurations.Models;

namespace Nebula.Modules.Configurations.Dto;

public class SystemConfigDto
{
    public Configuration Configuration { get; set; } = new Configuration();
    public User User { get; set; } = new User();
    public Roles Roles { get; set; } = new Roles();
}
