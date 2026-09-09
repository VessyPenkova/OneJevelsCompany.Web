using Microsoft.EntityFrameworkCore;
using OneJevelsCompany.Core.Entities;
using OneJevelsCompany.Core.Enums;
using Component = OneJevelsCompany.Core.Entities.Component;

namespace OneJevelsCompany.Infrastructure.Persistence
{
    public static class SeedData
    {
        public static async Task ApplyAsync(AppDbContext db)
        {
            async Task<ComponentCategory> EnsureCategoryAsync(
                string name,
                int sortOrder)
            {
                var category = await db.ComponentCategories
                    .FirstOrDefaultAsync(c => c.Name == name);

                if (category != null)
                {
                    return category;
                }

                category = new ComponentCategory
                {
                    Name = name,
                    SortOrder = sortOrder,
                    IsActive = true
                };

                db.ComponentCategories.Add(category);
                await db.SaveChangesAsync();

                return category;
            }

            async Task<Component> EnsureComponentAsync(
                string sku,
                Func<Component> createComponent)
            {
                var component = await db.Components
                    .FirstOrDefaultAsync(c => c.Sku == sku);

                if (component != null)
                {
                    return component;
                }

                component = createComponent();

                db.Components.Add(component);
                await db.SaveChangesAsync();

                return component;
            }

            async Task<Jewel> EnsureJewelAsync(
                string name,
                JewelCategory category,
                decimal basePrice,
                string imageUrl,
                int quantityOnHand)
            {
                var jewel = await db.Jewels
                    .FirstOrDefaultAsync(j => j.Name == name);

                if (jewel != null)
                {
                    return jewel;
                }

                jewel = new Jewel
                {
                    Name = name,
                    Category = category,
                    BasePrice = basePrice,
                    ImageUrl = imageUrl,
                    QuantityOnHand = quantityOnHand,
                    Components = new List<JewelComponent>()
                };

                db.Jewels.Add(jewel);
                await db.SaveChangesAsync();

                return jewel;
            }

            async Task EnsureJewelComponentAsync(
                Jewel jewel,
                Component component)
            {
                var exists = await db.JewelComponents
                    .AnyAsync(jc =>
                        jc.JewelId == jewel.Id &&
                        jc.ComponentId == component.Id);

                if (exists)
                {
                    return;
                }

                db.JewelComponents.Add(
                    new JewelComponent
                    {
                        JewelId = jewel.Id,
                        ComponentId = component.Id
                    });

                await db.SaveChangesAsync();
            }

            async Task EnsureDesignAsync(
                string name,
                JewelCategory category,
                string description,
                string imageUrl)
            {
                var exists = await db.Designs
                    .AnyAsync(d => d.Name == name);

                if (exists)
                {
                    return;
                }

                db.Designs.Add(
                    new Design
                    {
                        Name = name,
                        Category = category,
                        Description = description,
                        ImageUrl = imageUrl
                    });

                await db.SaveChangesAsync();
            }

            var catChain = await EnsureCategoryAsync(
                "Chain",
                10);

            var catClasp = await EnsureCategoryAsync(
                "Clasp",
                20);

            var catPendant = await EnsureCategoryAsync(
                "Pendant",
                30);

            var catBead = await EnsureCategoryAsync(
                "Bead",
                40);

            var goldChain = await EnsureComponentAsync(
                "CHN-G-45",
                () => new Component
                {
                    Name = "Gold Chain (45cm)",
                    ComponentCategoryId = catChain.Id,
                    Price = 120m,
                    Sku = "CHN-G-45",
                    ImageUrl = "/Images/Seed/chain-gold-45.jpg",
                    Dimensions = "45cm",
                    QuantityOnHand = 20
                });

            await EnsureComponentAsync(
                "CHN-S-45",
                () => new Component
                {
                    Name = "Silver Chain (45cm)",
                    ComponentCategoryId = catChain.Id,
                    Price = 40m,
                    Sku = "CHN-S-45",
                    ImageUrl = "/Images/Seed/chain-silver-45.jpg",
                    Dimensions = "45cm",
                    QuantityOnHand = 30
                });

            var leatherCord = await EnsureComponentAsync(
                "CORD-BLK",
                () => new Component
                {
                    Name = "Leather Cord (Black)",
                    ComponentCategoryId = catChain.Id,
                    Price = 20m,
                    Sku = "CORD-BLK",
                    ImageUrl = "/Images/Seed/cord-black.jpg",
                    Dimensions = "Adjustable",
                    QuantityOnHand = 40
                });

            var goldClasp = await EnsureComponentAsync(
                "CLASP-G",
                () => new Component
                {
                    Name = "Clasp – Gold",
                    ComponentCategoryId = catClasp.Id,
                    Price = 25m,
                    Sku = "CLASP-G",
                    ImageUrl = "/Images/Seed/clasp-gold.jpg",
                    Dimensions = "Std",
                    QuantityOnHand = 50
                });

            var silverClasp = await EnsureComponentAsync(
                "CLASP-S",
                () => new Component
                {
                    Name = "Clasp – Silver",
                    ComponentCategoryId = catClasp.Id,
                    Price = 10m,
                    Sku = "CLASP-S",
                    ImageUrl = "/Images/Seed/clasp-silver.jpg",
                    Dimensions = "Std",
                    QuantityOnHand = 60
                });

            await EnsureComponentAsync(
                "PEND-AMETH",
                () => new Component
                {
                    Name = "Pendant – Amethyst",
                    ComponentCategoryId = catPendant.Id,
                    Price = 60m,
                    Sku = "PEND-AMETH",
                    ImageUrl = "/Images/Seed/pendant-amethyst.jpg",
                    Dimensions = "Oval 18mm",
                    QuantityOnHand = 15,
                    Color = "Purple"
                });

            await EnsureComponentAsync(
                "PEND-EMRLD",
                () => new Component
                {
                    Name = "Pendant – Emerald",
                    ComponentCategoryId = catPendant.Id,
                    Price = 240m,
                    Sku = "PEND-EMRLD",
                    ImageUrl = "/Images/Seed/pendant-emerald.jpg",
                    Dimensions = "Pear 14mm",
                    QuantityOnHand = 8,
                    Color = "Green"
                });

            var pearlPendant = await EnsureComponentAsync(
                "PEND-PEARL",
                () => new Component
                {
                    Name = "Pendant – Pearl",
                    ComponentCategoryId = catPendant.Id,
                    Price = 85m,
                    Sku = "PEND-PEARL",
                    ImageUrl = "/Images/Seed/pendant-pearl.jpg",
                    Dimensions = "9mm",
                    QuantityOnHand = 18,
                    Color = "White"
                });

            var onyxBead = await EnsureComponentAsync(
                "BEAD-ONX",
                () => new Component
                {
                    Name = "Bead – Onyx (pack)",
                    ComponentCategoryId = catBead.Id,
                    Price = 15m,
                    Sku = "BEAD-ONX",
                    ImageUrl = "/Images/Seed/bead-onyx.jpg",
                    Dimensions = "6mm (pack)",
                    QuantityOnHand = 40,
                    Color = "Black",
                    SizeLabel = "6mm"
                });

            await EnsureComponentAsync(
                "BEAD-RQ",
                () => new Component
                {
                    Name = "Bead – Rose Quartz (pack)",
                    ComponentCategoryId = catBead.Id,
                    Price = 18m,
                    Sku = "BEAD-RQ",
                    ImageUrl = "/Images/Seed/bead-rose-quartz.jpg",
                    Dimensions = "6mm (pack)",
                    QuantityOnHand = 35,
                    Color = "Pink",
                    SizeLabel = "6mm"
                });

            await EnsureComponentAsync(
                "BEAD-LAPIS",
                () => new Component
                {
                    Name = "Bead – Lapis (pack)",
                    ComponentCategoryId = catBead.Id,
                    Price = 22m,
                    Sku = "BEAD-LAPIS",
                    ImageUrl = "/Images/Seed/bead-lapis.jpg",
                    Dimensions = "6mm (pack)",
                    QuantityOnHand = 32,
                    Color = "Blue",
                    SizeLabel = "6mm"
                });

            var necklace = await EnsureJewelAsync(
                "Classic Gold Pearl Necklace",
                JewelCategory.Necklace,
                0m,
                "/Images/Seed/jewel-gold-pearl-necklace.jpg",
                5);

            var bracelet = await EnsureJewelAsync(
                "Onyx Serenity Bracelet",
                JewelCategory.Bracelet,
                0m,
                "/Images/Seed/jewel-onyx-serenity-bracelet.jpg",
                7);

            await EnsureJewelComponentAsync(
                necklace,
                goldChain);

            await EnsureJewelComponentAsync(
                necklace,
                goldClasp);

            await EnsureJewelComponentAsync(
                necklace,
                pearlPendant);

            await EnsureJewelComponentAsync(
                bracelet,
                leatherCord);

            await EnsureJewelComponentAsync(
                bracelet,
                silverClasp);

            await EnsureJewelComponentAsync(
                bracelet,
                onyxBead);

            await EnsureDesignAsync(
                "Emerald Minimalist",
                JewelCategory.Necklace,
                "Elegant silver chain with emerald pendant.",
                "/Images/Seed/design-emerald-minimalist.jpg");

            await EnsureDesignAsync(
                "Ocean Lapis",
                JewelCategory.Necklace,
                "Delicate silver necklace with an ocean-inspired lapis pendant.",
                "/Images/Seed/design-ocean-lapis.jpg");
        }
    }
}