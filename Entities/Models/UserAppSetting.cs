using Entities.Enums;

namespace Entities.Models
{
    public class UserAppSetting : BaseEntity
    {
        public UserAppSettingKeyEnum Key { get; set; }
        public string Value { get; set; }
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    }
}
