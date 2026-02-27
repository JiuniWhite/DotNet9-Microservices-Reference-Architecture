using Catalog.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Data
{
    public class CatalogContext : DbContext
    {
        public CatalogContext(DbContextOptions<CatalogContext> options) : base(options) { }
        public DbSet<Service> Services { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Aquí podrías configurar relaciones si es necesario
            modelBuilder.HasDefaultSchema("Dbo");

            modelBuilder.Entity<Category>()
                .Property(c => c.Id)
                .UseIdentityAlwaysColumn(); // O UseIdentityByDefaultColumn()
        }
    }
}
