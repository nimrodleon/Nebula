using System.Linq;
using Microsoft.EntityFrameworkCore;
using Nebula.Data.Models;

namespace Nebula.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasQueryFilter(m => !m.SoftDeleted);
            base.OnModelCreating(modelBuilder);
        }
    }
}
