using System.ComponentModel;

namespace Entities.Enums
{
    public enum TransmissionEnum
    {
        [Description("")]
        Unknown = 0,

        [Description("Mexaniki")]
        Manual = 1,

        [Description("Avtomat")]
        Automatic = 2,

        [Description("Robot")]
        Robotic = 3,

        [Description("Variator")]
        CVT = 4,

        [Description("Reduktor")]
        Reducer = 6
    }
}
