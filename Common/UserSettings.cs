using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class UserSettings
    {
        public static string Token { get; set; }
        public static RoleEnum UserRole { get; set; }
    }
}
