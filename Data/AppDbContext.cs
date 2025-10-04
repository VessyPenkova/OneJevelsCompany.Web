using Microsoft.EntityFrameworkCore;
using OneJevelsCompany.Web.Models;
using Component = OneJevelsCompany.Web.Models.Component;

namespace OneJevelsCompany.Web.Data
{
    public class AppDbContext : DbContext
    {
       
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<JewelComponent>()
                .HasKey(jc => new { jc.JewelId, jc.ComponentId });

            modelBuilder.Entity<JewelComponent>()
                .HasOne(jc => jc.Jewel)
                .WithMany(j => j.Components)
                .HasForeignKey(jc => jc.JewelId);

            modelBuilder.Entity<JewelComponent>()
                .HasOne(jc => jc.Component)
                .WithMany(c => c.Jewels)
                .HasForeignKey(jc => jc.ComponentId);

            modelBuilder.Entity<Jewel>()
                .Property(j => j.BasePrice)
                .HasPrecision(14, 2);

            modelBuilder.Entity<Component>()
                .Property(c => c.Price)
                .HasPrecision(14, 2);

            modelBuilder.Entity<Order>()
                .Property(o => o.Total)
                .HasPrecision(14, 2);

            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.UnitPrice)
                .HasPrecision(14, 2);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Jewel> Jewels => Set<Jewel>();
        public DbSet<Component> Components => Set<Component>();
        public DbSet<JewelComponent> JewelComponents => Set<JewelComponent>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();
        public DbSet<Design> Designs => Set<Design>();

    }
}
