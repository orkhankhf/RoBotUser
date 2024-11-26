using NLog;
using System.Diagnostics;
using NLogLogger = NLog.ILogger;
using System.Text;
using Common.Resources;
using System.Runtime.InteropServices;

namespace Common.Helpers
{
    public static class AppHelper
    {
        private static readonly NLogLogger Logger = LogManager.GetCurrentClassLogger();

        public static void OpenUrlBrowser(string url)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true // Required to open URLs
            });
        }

        public static string GetActiveWindowName()
        {
            IntPtr handle = GetForegroundWindow();

            StringBuilder windowTitle = new StringBuilder(256); // Allocate buffer for the window title

            if (GetWindowText(handle, windowTitle, windowTitle.Capacity) > 0)
                return windowTitle.ToString();

            Logger.Error(LogMessagesRes.ActiveWindowNotFound);
            return "";
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
    }
}
