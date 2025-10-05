using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OneJevelsCompany.Web.Models;

namespace OneJevelsCompany.Web.Data
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Catalog
        public DbSet<ComponentCategory> ComponentCategories => Set<ComponentCategory>();
        public DbSet<Component> Components => Set<Component>();
        public DbSet<Jewel> Jewels => Set<Jewel>();
        public DbSet<JewelComponent> JewelComponents => Set<JewelComponent>();
        public DbSet<Design> Designs => Set<Design>();
        public DbSet<Collection> Collections => Set<Collection>();

        // Orders
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();

        // Inventory / Receiving
        public DbSet<Invoice> Invoices => Set<Invoice>();
        public DbSet<InvoiceLine> InvoiceLines => Set<InvoiceLine>();

        protected override void OnModelCreating(ModelBuilder model)
        {
            base.OnModelCreating(model); // IMPORTANT for Identity

            // ----- Junction: Jewel <-> Component (composite key)
            model.Entity<JewelComponent>()
                .HasKey(jc => new { jc.JewelId, jc.ComponentId });

            model.Entity<JewelComponent>()
                .HasOne(jc => jc.Jewel)
                .WithMany(j => j.Components)
                .HasForeignKey(jc => jc.JewelId)
                .OnDelete(DeleteBehavior.Cascade);

            model.Entity<JewelComponent>()
                .HasOne(jc => jc.Component)
                .WithMany(c => c.Jewels)
                .HasForeignKey(jc => jc.ComponentId)
                .OnDelete(DeleteBehavior.Cascade);

            // ----- Component -> Category (required FK)
            model.Entity<Component>()
                .HasOne(c => c.Category)
                .WithMany(cat => cat.Components)
                .HasForeignKey(c => c.ComponentCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // ----- InvoiceLine optional FKs (so we can invoice components, jewels or collections)
            model.Entity<InvoiceLine>()
                .HasOne(l => l.Component)
                .WithMany()
                .HasForeignKey(l => l.ComponentId)
                .OnDelete(DeleteBehavior.Restrict);

            model.Entity<InvoiceLine>()
                .HasOne(l => l.Jewel)
                .WithMany()
                .HasForeignKey(l => l.JewelId)
                .OnDelete(DeleteBehavior.Restrict);

            model.Entity<InvoiceLine>()
                .HasOne(l => l.Collection)
                .WithMany()
                .HasForeignKey(l => l.CollectionId)
                .OnDelete(DeleteBehavior.Restrict);

            // ----- Money / amounts precision
            model.Entity<Component>()
                .Property(c => c.Price)
                .HasPrecision(14, 2);

            model.Entity<Jewel>()
                .Property(j => j.BasePrice)
                .HasPrecision(14, 2);

            model.Entity<Collection>()
                .Property(c => c.BasePrice)
                .HasPrecision(14, 2);

            model.Entity<OrderItem>()
                .Property(oi => oi.UnitPrice)
                .HasPrecision(14, 2);

            model.Entity<Order>()
                .Property(o => o.Total)
                .HasPrecision(14, 2);

            model.Entity<InvoiceLine>()
                .Property(l => l.UnitCost)
                .HasPrecision(14, 2);
        }
    }
}
