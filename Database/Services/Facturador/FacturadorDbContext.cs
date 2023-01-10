using Microsoft.EntityFrameworkCore;

namespace Nebula.Database.Services.Facturador;

public class FacturadorDbContext : DbContext
{
    public FacturadorDbContext(DbContextOptions<FacturadorDbContext> options) : base(options) { }

    public DbSet<DocumentoFacturador> Documentos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DocumentoFacturador>().ToTable("DOCUMENTO");
        base.OnModelCreating(modelBuilder);
    }
}
