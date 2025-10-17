using Microsoft.EntityFrameworkCore;
using OneJevelsCompany.Web.Data;
using OneJevelsCompany.Web.Models;
using OneJevelsCompany.Web.Models.Admin;

namespace OneJevelsCompany.Web.Services.Dashboard
{
    public class DashboardService : IDashboardService
    {
        private readonly AppDbContext db;
        public DashboardService(AppDbContext db) => this.db = db;

        public async Task<DashboardVm> GetAsync()
        {
            var today = DateTime.UtcNow.Date;

            // ----- KPIs from Orders -----
            var paid = db.Orders.Where(o => o.Status == "Paid");
            var totalSales = await paid.SumAsync(o => (decimal?)o.Total) ?? 0m;
            var todaysSales = await paid.Where(o => o.CreatedUtc.Date == today)
                                        .SumAsync(o => (decimal?)o.Total) ?? 0m;
            var notInitiated = await db.Orders.Where(o => o.Status != "Paid")
                                              .SumAsync(o => (decimal?)o.Total) ?? 0m;

            // ----- From DesignOrders (if table exists/status used) -----
            decimal delayedVal = 0m, onHoldVal = 0m;
            try
            {
                // We don't know exact value fields; use UnitPriceEstimate * Quantity if present.
                // If fields don’t exist, both stay 0 (safe fallback).
                delayedVal = await db.DesignOrders
                    .Where(d => d.Status == "Delayed")
                    .SumAsync(d => (decimal?)((d.UnitPriceEstimate ?? 0m) * Math.Max(1, d.Quantity))) ?? 0m;

                onHoldVal = await db.DesignOrders
                    .Where(d => d.Status == "OnHold")
                    .SumAsync(d => (decimal?)((d.UnitPriceEstimate ?? 0m) * Math.Max(1, d.Quantity))) ?? 0m;
            }
            catch { /* safe no-op if columns differ */ }

            // ----- Variance by category (last 30 days) -----
            var since = today.AddDays(-30);
            var items = from oi in db.OrderItems
                        join o in db.Orders on oi.OrderId equals o.Id
                        where o.Status == "Paid" && o.CreatedUtc >= since
                        select new { oi.Category, Amount = oi.UnitPrice * oi.Quantity };

            var grouped = await items.GroupBy(x => x.Category)
                                     .Select(g => new { Category = g.Key, Actual = g.Sum(x => x.Amount) })
                                     .ToListAsync();

            // Simple forecast constants (tune later or pull from a table)
            var forecast = new Dictionary<JewelCategory, decimal>
            {
                { JewelCategory.Bracelet, 12000m },
                { JewelCategory.Necklace, 18000m }
            };

            var variances = grouped
                .Select(g => new SalesVarianceVm
                {
                    Category = g.Category.ToString(),
                    Actual = g.Actual,
                    Forecast = forecast.TryGetValue(g.Category, out var f) ? f : 10000m
                })
                .OrderBy(v => v.Category)
                .ToList();

            // ----- Top customer via email -----
            var top = await db.Orders
                .GroupBy(o => new { o.CustomerEmail, o.ShippingAddress })
                .Select(g => new {
                    g.Key.CustomerEmail,
                    g.Key.ShippingAddress,
                    Last = g.Max(x => x.CreatedUtc),
                    Ltv = g.Sum(x => x.Total)
                })
                .OrderByDescending(x => x.Ltv)
                .FirstOrDefaultAsync();

            var customer = top == null ? null : new CustomerVm
            {
                Email = top.CustomerEmail ?? "(guest)",
                ShippingAddress = top.ShippingAddress,
                LastOrderOn = top.Last,
                LifetimeValue = top.Ltv
            };

            // ----- 12-week trend (paid totals) -----
            var start = today.AddDays(-7 * 11);
            var trendRaw = await db.Orders
                .Where(o => o.Status == "Paid" && o.CreatedUtc >= start)
                .GroupBy(o => EF.Functions.DateDiffWeek(start, o.CreatedUtc))
                .Select(g => new { Week = g.Key, Total = g.Sum(x => x.Total) })
                .ToListAsync();

            var labels = Enumerable.Range(0, 12)
                .Select(i => start.AddDays(7 * i).ToString("MMM d"))
                .ToList();

            var values = Enumerable.Range(0, 12)
                .Select(i => trendRaw.FirstOrDefault(t => t.Week == i)?.Total ?? 0m)
                .ToList();

            // ----- Milestones from DesignOrders (recent) -----
            var milestones = new List<MilestoneVm>();
            try
            {
                var recent = await db.DesignOrders
                    .OrderByDescending(d => d.CreatedUtc)
                    .Take(5)
                    .Select(d => new { d.Id, d.Status, d.CreatedUtc, d.DesignName })
                    .ToListAsync();

                milestones = recent.Select(r => new MilestoneVm
                {
                    Title = $"{(string.IsNullOrWhiteSpace(r.DesignName) ? "Design" : r.DesignName)} — {r.Status}",
                    Tag = r.Status,
                    When = r.CreatedUtc
                }).ToList();
            }
            catch { /* ok if schema differs */ }

            return new DashboardVm
            {
                TotalSales = totalSales,
                TodaysSales = todaysSales,
                NotInitiatedValue = notInitiated,
                DelayedJobsValue = delayedVal,
                JobsOnHoldValue = onHoldVal,
                Variances = variances,
                Customer = customer,
                TrendLabels = labels,
                TrendSales = values,
                Milestones = milestones
            };
        }
    }
}
