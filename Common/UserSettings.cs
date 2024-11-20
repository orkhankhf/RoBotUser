using Entities.Enums;
using Entities.Models;

namespace Common
{
    public static class UserSettings
    {
        public static string Token { get; set; }

        public static RoleEnum UserRole { get; set; }

        public static List<Permission> Permissions { get; set; }

        public static List<UserAppSetting> UserAppSettings { get; set; }
    }
}
