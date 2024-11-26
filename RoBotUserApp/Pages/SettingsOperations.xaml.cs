using NLog;
using NLogLogger = NLog.ILogger;
using System.Windows.Controls;
using Common.Resources;
using Common;
using Entities.Enums;
using Common.Helpers;
using Entities.RequestModels;
using System.Windows;
using Entities.Models;
using RoBotUserApp.UiHelpers;

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
            ChromeTextBlock.Text = UIRes.SettingsOperations_ChromeTextBlock;
            WhatsAppTextBlock.Text = UIRes.SettingsOperations_WhatsAppTextBlock;
            WhatsappMessageUrlTextBlock.Text = UIRes.SettingsOperations_WhatsappMessageUrlTextBlock;
            SaveBtn.Content = UIRes.SettingsOperations_SaveBtn;
        }

        private async Task FillSettingsValues()
        {
            VoiceMessagePhoneNumberTextBox.Text = UserSettings.UserAppSettings
                .FirstOrDefault(m => m.Key == UserAppSettingKeyEnum.VoiceMessagePhoneNumber)?.Value;

            ChromeTextBox.Text = UserSettings.UserAppSettings
                .FirstOrDefault(m => m.Key == UserAppSettingKeyEnum.Chrome)?.Value;

            WhatsAppTextBox.Text = UserSettings.UserAppSettings
                .FirstOrDefault(m => m.Key == UserAppSettingKeyEnum.WhatsApp)?.Value;

            WhatsappMessageUrlTextBox.Text = UserSettings.UserAppSettings
                .FirstOrDefault(m => m.Key == UserAppSettingKeyEnum.WhatsappMessageUrl)?.Value;
        }

        #region TextBox And Button Events
        private async void SaveBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // Disable the entire UI
            UIHelper.ToggleMainGridState(true, DisableMainGrid);

            try
            {
                var voiceMessagePhoneNumberValue = VoiceMessagePhoneNumberTextBox.Text;

                if (string.IsNullOrWhiteSpace(voiceMessagePhoneNumberValue))
                {
                    UIHelper.Popup(PopupMessagesRes.EnterPhoneNumber, PopupMessagesRes.Title_ErrorOccured, MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Prepare the settings list
                var settings = new List<UserAppSettingRequest>
                {
                    new UserAppSettingRequest
                    {
                        Key = UserAppSettingKeyEnum.VoiceMessagePhoneNumber,
                        Value = voiceMessagePhoneNumberValue
                    }
                };

                // API request payload
                var request = new AddOrUpdateSettingRequest {
                    Settings = settings
                };

                // API call
                string apiUrl = "/UserAppSettings/AddOrUpdateSettings";
                var response = await RequestHelper.PostAsync<AddOrUpdateSettingRequest, OperationResult>(apiUrl, request);

                if (response.Success && response.Data.IsSuccessful)
                {
                    // Update static source UserSettings.UserAppSettings
                    foreach (var setting in settings)
                    {
                        var existingSetting = UserSettings.UserAppSettings.FirstOrDefault(us => us.Key == setting.Key);
                        if (existingSetting != null)
                        {
                            // Update existing setting value
                            existingSetting.Value = setting.Value;
                        }
                    }

                    UIHelper.Popup(PopupMessagesRes.General_ChangesSaved, PopupMessagesRes.Title_Attention, MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    UIHelper.Popup(PopupMessagesRes.General_UnexpectedErrorOccured, PopupMessagesRes.Title_ErrorOccured, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                UIHelper.Popup(PopupMessagesRes.General_UnexpectedErrorOccured, PopupMessagesRes.Title_ErrorOccured, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
            // Enable the entire UI
            UIHelper.ToggleMainGridState(false, DisableMainGrid); // Enable MainGrid
        }
        #endregion
    }
}
