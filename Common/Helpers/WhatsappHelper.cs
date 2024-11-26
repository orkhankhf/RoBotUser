using Common.Resources;
using Entities.Enums;
using InputSimulatorStandard;
using NLog;
using System.Runtime.InteropServices;
using NLogLogger = NLog.ILogger;

namespace Common.Helpers
{
    public static class WhatsappHelper
    {
        private static readonly NLogLogger Logger = LogManager.GetCurrentClassLogger();

        public static void BringWhatsAppToFrontAndResize(int xPosition, int yPosition, int width, int height)
        {
            // Find the WhatsApp window by its title
            var whatsapp = UserSettings.UserAppSettings.FirstOrDefault(m => m.Key == UserAppSettingKeyEnum.WhatsApp)?.Value;
            IntPtr whatsappHandle = FindWindow(null, whatsapp);

            if (whatsappHandle != IntPtr.Zero)
            {
                // Bring the WhatsApp window to the front
                SetForegroundWindow(whatsappHandle);

                // Resize and position the WhatsApp window
                SetWindowPos(whatsappHandle, IntPtr.Zero, xPosition, yPosition, width, height, SWP_SHOWWINDOW);
            }
            else
            {
                Logger.Error(string.Format(LogMessagesRes.WindowNotFound, whatsapp));
            }
        }

        public static async Task SendWhatsAppMessageAsync(string phoneNumber, string message, bool sendVoiceMessage)
        {
            var whatsapp = UserSettings.UserAppSettings.FirstOrDefault(m => m.Key == UserAppSettingKeyEnum.WhatsApp)?.Value;
            var voiceMessagePhoneNumber = UserSettings.UserAppSettings.FirstOrDefault(m => m.Key == UserAppSettingKeyEnum.VoiceMessagePhoneNumber)?.Value;

            BringWhatsAppToFrontAndResize(800, 500, 1200, 800);

            //click new chat
            await KeyMouseHelper.LeftClickRelativeToWindow(whatsapp, 297, 90, 600, 700);
            await TaskHelper.ShortDelay();

            //type phone number
            await KeyMouseHelper.EnterText(phoneNumber);
            await TaskHelper.ShortDelay();

            bool isWhatsappNumberAvailable = ScreenRecognitionHelper.IsWhatsappNumberAvailable();

            if (isWhatsappNumberAvailable)
            {
                await KeyMouseHelper.LeftClickRelativeToWindow(whatsapp, 380, 275, 600, 700);

                await KeyMouseHelper.LeftClickRelativeToWindow(whatsapp, 630, 760, 600, 700);

                await KeyMouseHelper.EnterText(message);

                KeyMouseHelper.PressEnter();

                if (sendVoiceMessage)
                    await RedirectVoiceMessage(voiceMessagePhoneNumber, phoneNumber);
            }
            else
            {
                //whatsapp is not available
                await KeyMouseHelper.LeftClickRelativeToWindow(whatsapp, 800, 770, 600, 700);
            }
        }

        public static async Task RedirectVoiceMessage(string from, string to)
        {
            var whatsappMessageUrl = UserSettings.UserAppSettings.FirstOrDefault(m => m.Key == UserAppSettingKeyEnum.WhatsappMessageUrl)?.Value;
            AppHelper.OpenUrlBrowser(string.Format(whatsappMessageUrl, from));

            var whatsapp = UserSettings.UserAppSettings.FirstOrDefault(m => m.Key == UserAppSettingKeyEnum.WhatsApp)?.Value;
            //after opening whatsapp in browser, wait until opens whatsapp app
            while (AppHelper.GetActiveWindowName() != whatsapp)
            {
                await TaskHelper.MediumDelay();
            }

            await TaskHelper.ShortDelay();

            BringWhatsAppToFrontAndResize(800, 500, 1200, 800);

            await TaskHelper.ShortDelay();

            InputSimulator sim = new InputSimulator();

            //right click to voice message
            await KeyMouseHelper.LeftClickRelativeToWindow(whatsapp, 560, 660, 600, 700);
            await KeyMouseHelper.RightClickRelativeToWindow(whatsapp, 560, 660, 600, 700);

            //click "Forward"
            await KeyMouseHelper.LeftDoubleClickRelativeToWindow(whatsapp, 590, 320, 600, 700);
            await TaskHelper.MediumDelay();

            //search number and forward
            await KeyMouseHelper.EnterText(to.Substring(1, to.Length - 3));//cut '+' symbol and last two digits and enter number to searchbox

            await TaskHelper.MediumDelay();

            //click phone number
            await KeyMouseHelper.LeftClickRelativeToWindow(whatsapp, 630, 320, 600, 700);

            //click "Forward"
            await KeyMouseHelper.LeftClickRelativeToWindow(whatsapp, 690, 260, 600, 700);
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetCursorPos(int x, int y);

        private const int SWP_SHOWWINDOW = 0x0040;
    }
}
