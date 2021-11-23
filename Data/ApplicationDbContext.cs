using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Nebula.Data.Models;

namespace Nebula.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Contact> Contacts { get; set; }
        public DbSet<PeopleDocType> PeopleDocTypes { get; set; }
        public DbSet<Caja> Cajas { get; set; }
        public DbSet<CajaDiaria> CajasDiaria { get; set; }
        public DbSet<UndMedida> UndMedida { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<CashierDetail> CashierDetails { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceDetail> InvoiceDetails { get; set; }
        public DbSet<SerieInvoice> SerieInvoices { get; set; }
        public DbSet<TypeOperationSunat> TypeOperationSunat { get; set; }
        public DbSet<InvoiceNote> InvoiceNotes { get; set; }
        public DbSet<InvoiceNoteDetail> InvoiceNoteDetails { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }

        /// <summary>
        /// Configurar SoftDelete.
        /// </summary>
        private void OnBeforeSaving()
        {
            foreach (var entry in ChangeTracker.Entries().Where(e =>
                e.Metadata.GetProperties().Any(x => x.Name == "SoftDeleted")))
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.CurrentValues["SoftDeleted"] = false;
                        break;
                    case EntityState.Modified:
                        entry.CurrentValues["SoftDeleted"] = false;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Unchanged;
                        entry.CurrentValues["SoftDeleted"] = true;
                        break;
                }
            }
        }

        /// <summary>
        /// SobreEscribir método guardar Cambios.
        /// </summary>
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        /// <summary>
        /// SobreEscribir método guardar Cambios Async.
        /// </summary>
        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            OnBeforeSaving();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        /// <summary>
        /// Configurar propiedades de los modelos.
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contact>().Property<bool>("SoftDeleted");
            modelBuilder.Entity<Contact>().HasQueryFilter(m => EF.Property<bool>(m, "SoftDeleted") == false);
            modelBuilder.Entity<PeopleDocType>().Property<bool>("SoftDeleted");
            modelBuilder.Entity<PeopleDocType>().HasQueryFilter(m => EF.Property<bool>(m, "SoftDeleted") == false);
            modelBuilder.Entity<Caja>().Property<bool>("SoftDeleted");
            modelBuilder.Entity<Caja>().HasQueryFilter(m => EF.Property<bool>(m, "SoftDeleted") == false);
            modelBuilder.Entity<CajaDiaria>().Property<bool>("SoftDeleted");
            modelBuilder.Entity<CajaDiaria>().HasQueryFilter(m => EF.Property<bool>(m, "SoftDeleted") == false);
            modelBuilder.Entity<UndMedida>().Property<bool>("SoftDeleted");
            modelBuilder.Entity<UndMedida>().HasQueryFilter(m => EF.Property<bool>(m, "SoftDeleted") == false);
            modelBuilder.Entity<Product>().Property<bool>("SoftDeleted");
            modelBuilder.Entity<Product>().HasQueryFilter(m => EF.Property<bool>(m, "SoftDeleted") == false);
            modelBuilder.Entity<CashierDetail>().Property<bool>("SoftDeleted");
            modelBuilder.Entity<CashierDetail>().HasQueryFilter(m => EF.Property<bool>(m, "SoftDeleted") == false);
            modelBuilder.Entity<Invoice>().Property<bool>("SoftDeleted");
            modelBuilder.Entity<Invoice>().HasQueryFilter(m => EF.Property<bool>(m, "SoftDeleted") == false);
            modelBuilder.Entity<InvoiceDetail>().Property<bool>("SoftDeleted");
            modelBuilder.Entity<InvoiceDetail>().HasQueryFilter(m => EF.Property<bool>(m, "SoftDeleted") == false);
            modelBuilder.Entity<SerieInvoice>().Property<bool>("SoftDeleted");
            modelBuilder.Entity<SerieInvoice>().HasQueryFilter(m => EF.Property<bool>(m, "SoftDeleted") == false);
            modelBuilder.Entity<InvoiceNote>().Property<bool>("SoftDeleted");
            modelBuilder.Entity<InvoiceNote>().HasQueryFilter(m => EF.Property<bool>(m, "SoftDeleted") == false);
            modelBuilder.Entity<InvoiceNoteDetail>().Property<bool>("SoftDeleted");
            modelBuilder.Entity<InvoiceNoteDetail>().HasQueryFilter(m => EF.Property<bool>(m, "SoftDeleted") == false);
            modelBuilder.Entity<Warehouse>().Property<bool>("SoftDeleted");
            modelBuilder.Entity<Warehouse>().HasQueryFilter(m => EF.Property<bool>(m, "SoftDeleted") == false);
            base.OnModelCreating(modelBuilder);
        }
    }
}
