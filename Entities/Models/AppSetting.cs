using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public class AppSetting : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string Key { get; set; }

        [StringLength(500)]
        public string Value { get; set; }

        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    }
}
