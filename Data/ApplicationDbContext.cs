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

        /// <summary>
        /// Lista de Contactos.
        /// </summary>
        public DbSet<Contact> Contacts { get; set; }

        /// <summary>
        /// Aperturas y Cierres de caja.
        /// </summary>
        public DbSet<CajaDiaria> CajasDiaria { get; set; }

        /// <summary>
        /// Unidades de Medida.
        /// </summary>
        public DbSet<UndMedida> UndMedida { get; set; }

        /// <summary>
        /// Lista de Productos.
        /// </summary>
        public DbSet<Product> Products { get; set; }

        /// <summary>
        /// Operaciones de Caja.
        /// </summary>
        public DbSet<CashierDetail> CashierDetails { get; set; }

        /// <summary>
        /// Cabecera Factura.
        /// </summary>
        public DbSet<Invoice> Invoices { get; set; }

        /// <summary>
        /// Detalle Factura.
        /// </summary>
        public DbSet<InvoiceDetail> InvoiceDetails { get; set; }

        /// <summary>
        /// Catalogo Tipos de Operación para facturar.
        /// </summary>
        public DbSet<TypeOperationSunat> TypeOperationSunat { get; set; }

        /// <summary>
        /// Notas de Crédito/Débito.
        /// </summary>
        public DbSet<InvoiceNote> InvoiceNotes { get; set; }

        /// <summary>
        /// Detalle Notas de Crédito/Débito.
        /// </summary>
        public DbSet<InvoiceNoteDetail> InvoiceNoteDetails { get; set; }

        // /// <summary>
        // /// Lista de almacenes.
        // /// </summary>
        // public DbSet<Warehouse> Warehouses { get; set; }

        /// <summary>
        /// Motivos de Inventario.
        /// </summary>
        public DbSet<InventoryReason> InventoryReasons { get; set; }

        /// <summary>
        /// Transferencia entre almacenes.
        /// </summary>
        public DbSet<TransferNote> TransferNotes { get; set; }

        /// <summary>
        /// Detalle Transferencia entre almacenes.
        /// </summary>
        public DbSet<TransferNoteDetail> TransferNoteDetails { get; set; }

        /// <summary>
        /// Notas de Ingreso/Salida.
        /// </summary>
        public DbSet<InventoryNote> InventoryNotes { get; set; }

        /// <summary>
        /// Detalle Notas de Ingreso/Salida.
        /// </summary>
        public DbSet<InventoryNoteDetail> InventoryNoteDetails { get; set; }

        // /// <summary>
        // /// Categoría para productos.
        // /// </summary>
        // public DbSet<Category> Categories { get; set; }

        /// <summary>
        /// Pendientes de Cobro/Pago.
        /// </summary>
        public DbSet<InvoiceAccount> InvoiceAccounts { get; set; }

        /// <summary>
        /// Configuración del sistema.
        /// </summary>
        public DbSet<Configuration> Configuration { get; set; }

        /// <summary>
        /// Series de facturación.
        /// </summary>
        public DbSet<InvoiceSerie> InvoiceSeries { get; set; }

        /// <summary>
        /// Tributos Generales de la Factura.
        /// </summary>
        public DbSet<Tributo> Tributos { get; set; }

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
            modelBuilder.Entity<InvoiceNote>().Property<bool>("SoftDeleted");
            modelBuilder.Entity<InvoiceNote>().HasQueryFilter(m => EF.Property<bool>(m, "SoftDeleted") == false);
            // modelBuilder.Entity<Warehouse>().Property<bool>("SoftDeleted");
            // modelBuilder.Entity<Warehouse>().HasQueryFilter(m => EF.Property<bool>(m, "SoftDeleted") == false);
            modelBuilder.Entity<TransferNote>().Property<bool>("SoftDeleted");
            modelBuilder.Entity<TransferNote>().HasQueryFilter(m => EF.Property<bool>(m, "SoftDeleted") == false);
            modelBuilder.Entity<InventoryNote>().Property<bool>("SoftDeleted");
            modelBuilder.Entity<InventoryNote>().HasQueryFilter(m => EF.Property<bool>(m, "SoftDeleted") == false);
            // modelBuilder.Entity<Category>().Property<bool>("SoftDeleted");
            // modelBuilder.Entity<Category>().HasQueryFilter(m => EF.Property<bool>(m, "SoftDeleted") == false);
            modelBuilder.Entity<Configuration>().Property<bool>("SoftDeleted");
            modelBuilder.Entity<Configuration>().HasQueryFilter(m => EF.Property<bool>(m, "SoftDeleted") == false);
            modelBuilder.Entity<InvoiceSerie>().Property<bool>("SoftDeleted");
            modelBuilder.Entity<InvoiceSerie>().HasQueryFilter(m => EF.Property<bool>(m, "SoftDeleted") == false);
            base.OnModelCreating(modelBuilder);
        }
    }
}
