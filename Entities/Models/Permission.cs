using Entities.Enums;

namespace Entities.Models
{
    public class Permission : BaseEntity
    {
        public PermissionEnum PermissionFor { get; set; }
        public int Value { get; set; }
    }
}
