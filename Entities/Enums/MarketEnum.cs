using System.ComponentModel;

namespace Entities.Enums
{
    public enum MarketEnum
    {
        [Description("")]
        Unknown = 0,

        [Description("Amerika")]
        America = 1,

        [Description("Avropa")]
        Europe = 2,

        [Description("Yaponiya")]
        Japan = 4,

        [Description("Koreya")]
        Korea = 5,

        [Description("Dubay")]
        Dubai = 6,

        [Description("Rəsmi diler")]
        OfficialDealer = 7,

        [Description("Rusiya")]
        Russia = 8,

        [Description("Digər")]
        Other = 9,

        [Description("Çin")]
        China = 42
    }
}
