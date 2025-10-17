namespace OneJevelsCompany.Web.Models.Admin
{
    public class DashboardVm
    {
        // KPIs
        public decimal TotalSales { get; set; }
        public decimal TodaysSales { get; set; }
        public decimal NotInitiatedValue { get; set; }   // Orders not Paid
        public decimal DelayedJobsValue { get; set; }    // from DesignOrders (if any)
        public decimal JobsOnHoldValue { get; set; }     // from DesignOrders (if any)

        // Variance by category (Orders -> OrderItems)
        public List<SalesVarianceVm> Variances { get; set; } = new();

        // “Customer” (grouped by CustomerEmail)
        public CustomerVm? Customer { get; set; }
        public List<MilestoneVm> Milestones { get; set; } = new();

        // Chart
        public List<string> TrendLabels { get; set; } = new();
        public List<decimal> TrendSales { get; set; } = new();
    }
}
