using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.RequestModels
{
    public class GetRoleByTokenResponse
    {
        public RoleEnum Role { get; set; }
        public bool IsFound { get; set; }
        public bool IsValid { get; set; }
    }
}
