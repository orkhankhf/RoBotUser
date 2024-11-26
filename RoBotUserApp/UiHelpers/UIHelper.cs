using System.Windows;

namespace RoBotUserApp.UiHelpers
{
    public static class UIHelper
    {
        public static void Popup(string message, string title, MessageBoxButton messageBoxButton, MessageBoxImage messageBoxImage)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                MessageBox.Show(message, title, messageBoxButton, messageBoxImage);
            });
        }

        public static void ToggleMainGridState(bool action, EventHandler<bool> disableMainGrid)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                disableMainGrid?.Invoke(null, action);
            });
        }
    }
}
