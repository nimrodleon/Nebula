using Microsoft.EntityFrameworkCore;

namespace Nebula.Database.Services.Facturador;

public class FacturadorDbContext : DbContext
{
    public DbSet<DocumentoFacturador> Documentos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DocumentoFacturador>()
            .ToTable("DOCUMENTO");
        modelBuilder.Entity<DocumentoFacturador>()
            .HasKey(x => new
            {
                x.NUM_RUC,
                x.TIP_DOCU,
                x.NUM_DOCU
            });
        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=C:/SFS_v1.6/bd/BDFacturador.db");
        base.OnConfiguring(optionsBuilder);
    }
}
