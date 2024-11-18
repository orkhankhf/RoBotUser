using System.ComponentModel;

namespace Entities.Enums
{
    public enum GearEnum
    {
        [Description("")]
        Unknown = 0,

        [Description("Arxa")]
        Rear = 1,

        [Description("Ön")]
        Front = 2,

        [Description("Tam")]
        AllWheel = 3
    }
}
