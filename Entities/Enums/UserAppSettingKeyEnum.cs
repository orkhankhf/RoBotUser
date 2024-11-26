using System.ComponentModel;

namespace Entities.Enums
{
    public enum UserAppSettingKeyEnum
    {
        [Description("Səs mesajı yönləndiriləcək nömrə")]
        VoiceMessagePhoneNumber = 1,

        [Description("Browser")]
        Chrome = 2,

        [Description("Whatsapp")]
        WhatsApp = 3,

        [Description("Whatsapp URL")]
        WhatsappMessageUrl = 4
    }
}
