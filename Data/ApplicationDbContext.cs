using Microsoft.EntityFrameworkCore;
using Nebula.Data.Models;

namespace Nebula.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Contact> Contacts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasQueryFilter(m => !m.SoftDeleted);
            modelBuilder.Entity<Contact>().HasQueryFilter(m => !m.SoftDeleted);
            base.OnModelCreating(modelBuilder);
        }
    }
}
