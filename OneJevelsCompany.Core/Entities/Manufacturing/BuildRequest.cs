using OneJevelsCompany.Core.Enums;

namespace OneJevelsCompany.Core.Entities.Manufacturing
{
    public class BuildRequest
    {
        public JewelCategory Category { get; set; }

        public List<int> ComponentIds { get; set; } = new();
    }
}