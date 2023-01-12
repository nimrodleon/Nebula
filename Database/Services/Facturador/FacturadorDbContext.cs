using Microsoft.EntityFrameworkCore;

namespace Nebula.Database.Services.Facturador;

public class FacturadorDbContext : DbContext
{
    public DbSet<DocumentoFacturador> Documentos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=C:\\SFS_v1.6\\bd\\BDFacturador.db");
        base.OnConfiguring(optionsBuilder);
    }
}
