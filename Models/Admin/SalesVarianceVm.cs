namespace OneJevelsCompany.Web.Models.Admin
{
    public class SalesVarianceVm
    {
        public string Category { get; set; } = "";
        public decimal Forecast { get; set; }
        public decimal Actual { get; set; }
        public decimal Variance => Actual - Forecast;
    }
}
