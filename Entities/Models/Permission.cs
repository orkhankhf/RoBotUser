using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Permission : BaseEntity
    {
        public PermissionEnum PermissionFor { get; set; }
        public int Value { get; set; }
    }
}
