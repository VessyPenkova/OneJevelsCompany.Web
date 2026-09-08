using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OneJevelsCompany.Core.Entities;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace OneJevelsCompany.Infrastructure.Persistence
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<ComponentCategory> ComponentCategories => Set<ComponentCategory>();
        public DbSet<Component> Components => Set<Component>();
        public DbSet<Jewel> Jewels => Set<Jewel>();
        public DbSet<JewelComponent> JewelComponents => Set<JewelComponent>();
        public DbSet<Design> Designs => Set<Design>();
        public DbSet<Collection> Collections => Set<Collection>();
        public DbSet<PurchaseNeed> PurchaseNeeds => Set<PurchaseNeed>();

        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();

        public DbSet<Invoice> Invoices => Set<Invoice>();
        public DbSet<InvoiceLine> InvoiceLines => Set<InvoiceLine>();

        public DbSet<DesignOrder> DesignOrders => Set<DesignOrder>();

        protected override void OnModelCreating(ModelBuilder model)
        {
            base.OnModelCreating(model);

            model.Entity<JewelComponent>()
                .HasKey(jc => new
                {
                    jc.JewelId,
                    jc.ComponentId
                });

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

            model.Entity<Component>()
                .HasOne(c => c.Category)
                .WithMany(c => c.Components)
                .HasForeignKey(c => c.ComponentCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

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

            model.Entity<DesignOrder>()
                .Property(d => d.UnitPriceEstimate)
                .HasPrecision(14, 2);

            model.Entity<DesignOrder>()
                .Property(d => d.LengthCm)
                .HasPrecision(6, 2);

            model.Entity<DesignOrder>()
                .HasIndex(d => d.CreatedUtc);

            model.Entity<DesignOrder>()
                .HasIndex(d => d.Status);

            model.Entity<Component>()
                .Property(c => c.MinOrderQty)
                .HasDefaultValue(120);

            model.Entity<PurchaseNeed>(b =>
            {
                b.ToTable("PurchaseNeeds");

                b.HasKey(p => p.Id);

                b.Property(p => p.NeededQty)
                    .HasDefaultValue(0);

                b.Property(p => p.MinOrderQtyUsed)
                    .HasDefaultValue(0);

                b.Property(p => p.CreatedUtc)
                    .HasDefaultValueSql("GETUTCDATE()");

                b.Property(p => p.LastUpdatedUtc)
                    .HasDefaultValueSql("GETUTCDATE()");

                b.HasOne(p => p.Component)
                    .WithMany()
                    .HasForeignKey(p => p.ComponentId)
                    .OnDelete(DeleteBehavior.Cascade);

                b.HasIndex(p => p.ComponentId);
            });
        }
    }
}