using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Talabat.Core.Entities;

namespace Talabat.Repository.Data
{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options)
            : base(options) { }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductBrand> ProductBrands { get; set; }

        public DbSet<ProductType> ProductTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(assembly: Assembly.GetExecutingAssembly());

        }
    }
}
