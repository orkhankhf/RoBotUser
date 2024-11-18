using InputSimulatorStandard.Native;
using InputSimulatorStandard;
using System.Runtime.InteropServices;
using NLogLogger = NLog.ILogger;
using NLog;
using Common.Resources;
using TextCopy;

namespace Common.Helpers
{
    public static class KeyMouseHelper
    {
        private static readonly NLogLogger Logger = LogManager.GetCurrentClassLogger();

        #region Mouse Events
        //Left Click
        public static async Task LeftClick(int xPosition, int yPosition, int delayMin, int delayMax)
        {
            InputSimulator sim = new InputSimulator();

            SetCursorPos(xPosition, yPosition);
            await TaskHelper.DynamicDelay(delayMin, delayMax);
            sim.Mouse.LeftButtonClick();
        }

        public static async Task LeftClickRelativeToWindow(string windowTitle, int relativeX, int relativeY, int delayMin, int delayMax)
        {
            // Find the window handle by title
            IntPtr hWnd = FindWindow(null, windowTitle);

            if (hWnd == IntPtr.Zero)
                Logger.Warn(string.Format(LogMessagesRes.WindowNotFound, windowTitle));
            

            // Get the window's position and dimensions
            if (GetWindowRect(hWnd, out RECT rect))
            {
                // Calculate absolute coordinates based on the window's position
                int absoluteX = rect.Left + relativeX;
                int absoluteY = rect.Top + relativeY;

                // Perform the click at the calculated coordinates
                await LeftClick(absoluteX, absoluteY, delayMin, delayMax);
            }
            else
            {
                Logger.Error(string.Format(LogMessagesRes.FailedGetWindowRectangle, windowTitle));
            }
        }

        public static async Task LeftDoubleClick(int xPosition, int yPosition, int delayMin, int delayMax)
        {
            Random random = new Random();
            InputSimulator sim = new InputSimulator();

            SetCursorPos(xPosition, yPosition);
            await TaskHelper.DynamicDelay(delayMin, delayMax);
            sim.Mouse.LeftButtonDoubleClick();
        }

        public static async Task LeftDoubleClickRelativeToWindow(string windowTitle, int relativeX, int relativeY, int delayMin, int delayMax)
        {
            // Find the window handle by title
            IntPtr hWnd = FindWindow(null, windowTitle);

            if (hWnd == IntPtr.Zero)
            {
                Logger.Warn(string.Format(LogMessagesRes.WindowNotFound, windowTitle));
                return; // Exit if the window is not found
            }

            // Get the window's position and dimensions
            if (GetWindowRect(hWnd, out RECT rect))
            {
                // Calculate absolute coordinates based on the window's position
                int absoluteX = rect.Left + relativeX;
                int absoluteY = rect.Top + relativeY;

                // Perform the left double-click at the calculated coordinates
                await LeftDoubleClick(absoluteX, absoluteY, delayMin, delayMax);
            }
            else
            {
                Logger.Error(string.Format(LogMessagesRes.FailedGetWindowRectangle, windowTitle));
            }
        }
        //Right Click

        public static async Task RightClick(int xPosition, int yPosition, int delayMin, int delayMax)
        {
            InputSimulator sim = new InputSimulator();

            SetCursorPos(xPosition, yPosition);
            await TaskHelper.DynamicDelay(delayMin, delayMax);
            sim.Mouse.RightButtonClick();
        }

        public static async Task RightClickRelativeToWindow(string windowTitle, int relativeX, int relativeY, int delayMin, int delayMax)
        {
            // Find the window handle by title
            IntPtr hWnd = FindWindow(null, windowTitle);

            if (hWnd == IntPtr.Zero)
                Logger.Warn(string.Format(LogMessagesRes.WindowNotFound, windowTitle));
            

            // Get the window's position and dimensions
            if (GetWindowRect(hWnd, out RECT rect))
            {
                // Calculate absolute coordinates based on the window's position
                int absoluteX = rect.Left + relativeX;
                int absoluteY = rect.Top + relativeY;

                // Perform the right-click at the calculated coordinates
                await RightClick(absoluteX, absoluteY, delayMin, delayMax);
            }
            else
            {
                Logger.Error(string.Format(LogMessagesRes.FailedGetWindowRectangle, windowTitle));
            }
        }
        #endregion


        #region Keyboard Events
        public static async Task EnterText(string message)
        {
            InputSimulator sim = new InputSimulator();

            // Copy the message to the clipboard
            ClipboardService.SetText(message);

            // Simulate Ctrl + V to paste the text
            sim.Keyboard.KeyDown(VirtualKeyCode.CONTROL);
            sim.Keyboard.KeyPress(VirtualKeyCode.VK_V);
            sim.Keyboard.KeyUp(VirtualKeyCode.CONTROL);

            // Add a short delay to allow the paste action to complete
            await TaskHelper.ShortDelay();
        }

        public static void PressEnter()
        {
            InputSimulator sim = new InputSimulator();
            sim.Keyboard.KeyPress(VirtualKeyCode.RETURN);
        }
        #endregion



        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetCursorPos(int x, int y);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }
    }
}
