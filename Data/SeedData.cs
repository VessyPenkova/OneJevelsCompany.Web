using Microsoft.EntityFrameworkCore;
using OneJevelsCompany.Web.Models;
using System.ComponentModel;
using Component = OneJevelsCompany.Web.Models.Component;

namespace OneJevelsCompany.Web.Data
{
    public static class SeedData
    {
        public static async Task ApplyAsync(AppDbContext db)
        {
            if (await db.Jewels.AnyAsync()) return;

            var components = new List<Component>
            {
                new Component { Name = "Gold Chain (45cm)", Type = ComponentType.Chain, Price = 120m },
                new Component { Name = "Silver Chain (45cm)", Type = ComponentType.Chain, Price = 40m },
                new Component { Name = "Leather Cord (Black)", Type = ComponentType.Chain, Price = 20m },

                new Component { Name = "Clasp – Gold", Type = ComponentType.Clasp, Price = 25m },
                new Component { Name = "Clasp – Silver", Type = ComponentType.Clasp, Price = 10m },

                new Component { Name = "Pendant – Amethyst", Type = ComponentType.Pendant, Price = 60m },
                new Component { Name = "Pendant – Emerald", Type = ComponentType.Pendant, Price = 240m },
                new Component { Name = "Pendant – Pearl", Type = ComponentType.Pendant, Price = 85m },

                new Component { Name = "Bead – Onyx (pack)", Type = ComponentType.Bead, Price = 15m },
                new Component { Name = "Bead – Rose Quartz (pack)", Type = ComponentType.Bead, Price = 18m },
                new Component { Name = "Bead – Lapis (pack)", Type = ComponentType.Bead, Price = 22m }
            };
            db.Components.AddRange(components);

            var ready = new List<Jewel>
            {
                new Jewel
                {
                    Name = "Classic Gold Pearl Necklace",
                    Category = JewelCategory.Necklace,
                    BasePrice = 0m,
                    Components = new List<JewelComponent>()
                },
                new Jewel
                {
                    Name = "Onyx Serenity Bracelet",
                    Category = JewelCategory.Bracelet,
                    BasePrice = 0m,
                    Components = new List<JewelComponent>()
                }
            };
            db.Jewels.AddRange(ready);
            await db.SaveChangesAsync();

            // Tie some components to ready-made pieces (their price = sum of components)
            var necklace = ready.First(j => j.Category == JewelCategory.Necklace);
            var bracelet = ready.First(j => j.Category == JewelCategory.Bracelet);

            void AddComp(Jewel j, string compName)
            {
                var c = components.First(x => x.Name == compName);
                db.JewelComponents.Add(new JewelComponent { JewelId = j.Id, ComponentId = c.Id });
            }

            AddComp(necklace, "Gold Chain (45cm)");
            AddComp(necklace, "Clasp – Gold");
            AddComp(necklace, "Pendant – Pearl");

            AddComp(bracelet, "Leather Cord (Black)");
            AddComp(bracelet, "Clasp – Silver");
            AddComp(bracelet, "Bead – Onyx (pack)");

            await db.SaveChangesAsync();

            // Best designs showcase
            db.Designs.AddRange(
                new Design { Name = "Emerald Minimalist", Category = JewelCategory.Necklace, Description = "Elegant gold chain with emerald pendant." },
                new Design { Name = "Ocean Lapis", Category = JewelCategory.Bracelet, Description = "Lapis beads on leather with silver clasp." }
            );

            await db.SaveChangesAsync();
        }
    }
}
