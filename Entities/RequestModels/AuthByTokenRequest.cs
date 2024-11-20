using Entities.Enums;
using Entities.Models;

namespace Entities.RequestModels
{
    public class AuthByTokenResponse
    {
        public RoleEnum Role { get; set; }
        public bool IsFound { get; set; }
        public bool IsValid { get; set; }
        public List<Permission> Permissions { get; set; }
        public List<UserAppSetting> UserAppSettings { get; set; }
    }
}
