using Entities.Enums;

namespace Entities.RequestModels
{
    public class GetRoleByTokenResponse
    {
        public RoleEnum Role { get; set; }
        public bool IsFound { get; set; }
        public bool IsValid { get; set; }
    }
}
