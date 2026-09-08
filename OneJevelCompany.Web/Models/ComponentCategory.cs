using System.ComponentModel.DataAnnotations;

namespace OneJevelsCompany.Web.Models
{
    public class ComponentCategory
    {
        public int Id { get; set; }

        [Required, MaxLength(80)]
        public string Name { get; set; } = string.Empty;

        public int SortOrder { get; set; } = 0;
        public bool IsActive { get; set; } = true;

        public ICollection<Component> Components { get; set; } = new List<Component>();
    }
}
