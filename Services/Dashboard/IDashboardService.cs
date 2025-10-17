using OneJevelsCompany.Web.Models.Admin;

namespace OneJevelsCompany.Web.Services.Dashboard
{
    public interface IDashboardService
    {
        Task<DashboardVm> GetAsync();
    }
}
