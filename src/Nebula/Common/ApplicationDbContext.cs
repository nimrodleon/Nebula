using Microsoft.EntityFrameworkCore;
using Nebula.Common.Models;
using Nebula.Modules.Auth.Models;

namespace Nebula.Common;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<CodigoUbigeo> CodigosUbigeo { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
}

