using Common.Resources;
using Entities.Models;
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
            IntPtr whatsappHandle = FindWindow(null, AppSettings.App.WhatsApp);

            if (whatsappHandle != IntPtr.Zero)
            {
                // Bring the WhatsApp window to the front
                SetForegroundWindow(whatsappHandle);

                // Resize and position the WhatsApp window
                SetWindowPos(whatsappHandle, IntPtr.Zero, xPosition, yPosition, width, height, SWP_SHOWWINDOW);
            }
            else
            {
                Logger.Error(string.Format(LogMessagesRes.WindowNotFound, AppSettings.App.WhatsApp));
            }
        }

        public static async Task RedirectVoiceMessage(string from, string to)
        {
            AppHelper.OpenUrlBrowser(string.Format(AppSettings.App.WhatsappMessageUrl, from));
            
            //after opening whatsapp in browser, wait until opens whatsapp app
            while (AppHelper.GetActiveWindowName() != AppSettings.App.WhatsApp)
            {
                await TaskHelper.MediumDelay();
            }

            await TaskHelper.ShortDelay();

            BringWhatsAppToFrontAndResize(800, 500, 1200, 800);

            await TaskHelper.ShortDelay();

            InputSimulator sim = new InputSimulator();

            //right click to voice message
            await KeyMouseHelper.LeftClickRelativeToWindow(AppSettings.App.WhatsApp, 560, 660, 600, 700);
            await KeyMouseHelper.RightClickRelativeToWindow(AppSettings.App.WhatsApp, 560, 660, 600, 700);

            //click "Forward"
            await KeyMouseHelper.LeftDoubleClickRelativeToWindow(AppSettings.App.WhatsApp, 590, 320, 600, 700);
            await TaskHelper.MediumDelay();

            //search number and forward
            await KeyMouseHelper.EnterText(to.Substring(1, to.Length - 3));//cut '+' symbol and last two digits and enter number to searchbox

            await TaskHelper.MediumDelay();

            //click phone number
            await KeyMouseHelper.LeftClickRelativeToWindow(AppSettings.App.WhatsApp, 630, 320, 600, 700);

            //click "Forward"
            await KeyMouseHelper.LeftClickRelativeToWindow(AppSettings.App.WhatsApp, 690, 260, 600, 700);
        }

        public static async Task SendWhatsAppMessageAsync(string message, bool sendVoiceMessage)
        {
            //string formattedNumber = AppHelper.FormatPhoneNumberToCountryCode(adInformation.PhoneNumber);

            ////test number
            ////formattedNumber = "+994702972111";

            //if (!string.IsNullOrEmpty(formattedNumber))
            //{
            //    BringWhatsAppToFrontAndResize(800, 500, 1200, 800);

            //    //click new chat
            //    await KeyMouseHelper.LeftClickRelativeToWindow(AppSettings.App.WhatsApp, 297, 90, 600, 700);
            //    await TaskHelper.ShortDelay();

            //    //type phone number
            //    await KeyMouseHelper.EnterText(formattedNumber);
            //    await TaskHelper.ShortDelay();

            //    bool isWhatsappNumberAvailable = ScreenRecognitionHelper.IsWhatsappNumberAvailable();

            //    if (isWhatsappNumberAvailable)
            //    {
            //        await KeyMouseHelper.LeftClickRelativeToWindow(AppSettings.App.WhatsApp, 380, 275, 600, 700);

            //        await KeyMouseHelper.LeftClickRelativeToWindow(AppSettings.App.WhatsApp, 630, 760, 600, 700);

            //        await KeyMouseHelper.EnterText(message + "\r\n\r\n" + adInformation.Url);

            //        KeyMouseHelper.PressEnter();

            //        if (sendVoiceMessage)
            //            await RedirectVoiceMessage(AppSettings.App.RedirectVoiceMessageNumber, formattedNumber);
            //    }
            //    else
            //    {
            //        //whatsapp is not available
            //        await KeyMouseHelper.LeftClickRelativeToWindow(AppSettings.App.WhatsApp, 800, 770, 600, 700);
            //    }
            //}
            //else
            //{
            //    Logger.Error(string.Format(LogMessagesRes.FormattedPhoneIsInvalid, formattedNumber));
            //}
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
