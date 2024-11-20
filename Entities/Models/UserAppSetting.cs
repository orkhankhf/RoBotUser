using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class UserAppSetting : BaseEntity
    {
        public UserAppSettingKeyEnum Key { get; set; }
        public string Value { get; set; }
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    }
}
