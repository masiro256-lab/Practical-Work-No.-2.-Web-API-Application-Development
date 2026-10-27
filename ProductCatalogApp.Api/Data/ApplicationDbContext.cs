using Microsoft.EntityFrameworkCore;
using ProductCatalogApp.Api.Models;

namespace ProductCatalogApp.Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ProductionSite> ProductionSites =>
            Set<ProductionSite>();

        public DbSet<Equipment> EquipmentItems =>
            Set<Equipment>();
    }
}
