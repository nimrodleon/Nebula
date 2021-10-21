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
            base.OnModelCreating(modelBuilder);
        }
    }
}
