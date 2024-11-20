using NLog;
using NLogLogger = NLog.ILogger;
using System.Windows.Controls;
using Common.Resources;
using Common;
using Entities.Enums;

namespace RoBotUserApp.Pages
{
    public partial class SettingsOperations : UserControl
    {
        private static readonly NLogLogger Logger = LogManager.GetCurrentClassLogger();
        public event EventHandler<bool> DisableMainGrid;
        public SettingsOperations()
        {
            InitializeComponent();
            _ = FillUITextsAsync();
            _ = FillSettingsValues();
        }

        private async Task FillUITextsAsync()
        {
            VoiceMessagePhoneNumberTextBlock.Text = UIRes.SettingsOperations_VoiceMessagePhoneNumberTextBlock;
            SaveBtn.Content = UIRes.SettingsOperations_SaveBtn;
        }

        private async Task FillSettingsValues()
        {
            VoiceMessagePhoneNumberTextBox.Text = UserSettings.UserAppSettings
                .FirstOrDefault(m => m.Key == UserAppSettingKeyEnum.VoiceMessagePhoneNumber)?.Value;
        }

        #region TextBox And Button Events
        private void SaveBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }
        #endregion
    }
}
