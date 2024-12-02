using NLog;
using NLogLogger = NLog.ILogger;
using System.Windows.Controls;
using System.Windows;
using Common.Resources;
using Common.Helpers;
using Entities.RequestModels;
using Entities.Models;
using Common;
using Entities.Enums;
using RoBotUserApp.UiHelpers;

namespace RoBotUserApp.Pages
{
    public partial class WhatsappOperations : UserControl
    {
        private static readonly NLogLogger Logger = LogManager.GetCurrentClassLogger();
        public event EventHandler<bool> DisableMainGrid;
        public Action<bool> CanSendVoiceMessage { get; set; }
        public static bool SendVoiceMessage { get; set; }

        public WhatsappOperations()
        {
            InitializeComponent();
            _ = FillUITextsAsync();
        }

        private async Task FillUITextsAsync()
        {
            SendMessagesBtn.Content = UIRes.WhatsappOperations_SendMessagesBtn;
            SendVoiceMessageText.Text = UIRes.WhatsappOperations_SendVoiceMessageText;
        }

        private static async Task<List<Message>> GetMessageTemplates()
        {
            try
            {
                string apiUrl = $"/Messages/User/";
                var response = await RequestHelper.GetAsync<List<Message>>(apiUrl);

                if (response.Success)
                {
                    return response.Data;
                }
                else
                {
                    UIHelper.Popup(PopupMessagesRes.CantLoadMessageTemplates, PopupMessagesRes.Title_ErrorOccured, MessageBoxButton.OK, MessageBoxImage.Error);
                    return new List<Message>();
                }
            }
            catch (Exception ex)
            {
                UIHelper.Popup("Mesajları yükləyərkən bir xəta baş verdi.", PopupMessagesRes.Title_ErrorOccured, MessageBoxButton.OK, MessageBoxImage.Error);
                return new List<Message>();
            }
        }

        #region TextBox And Button Events
        private async void SendMessagesBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // Disable the entire UI
            UIHelper.ToggleMainGridState(true, DisableMainGrid);

            // Check if the current time is within the restricted range
            if (DateTimeHelper.IsRestrictedTimeToSendMessage())
            {
                UIHelper.Popup(PopupMessagesRes.RestrictedTimeToSendMessage, PopupMessagesRes.Title_Attention, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Simulate API call to get phone numbers which not sent message
            string apiUrl = "/AssignedPhoneNumber/GetUnsentPhoneNumbers";
            var response = await RequestHelper.GetAsync<GetUnsentPhoneNumbersResponse>(apiUrl);

            if (response.Success)
            {
                var messages = await GetMessageTemplates();

                if (messages.Count == 0)
                {
                    UIHelper.Popup(PopupMessagesRes.NoExistingMessageTemplates, PopupMessagesRes.Title_Attention, MessageBoxButton.OK, MessageBoxImage.Warning);
                    // Enable the entire UI
                    UIHelper.ToggleMainGridState(false, DisableMainGrid);
                    return;
                }
                var voiceMessagePhoneNumber = UserSettings.UserAppSettings.FirstOrDefault(m => m.Key == UserAppSettingKeyEnum.VoiceMessagePhoneNumber)?.Value;

                if (SendVoiceMessage && voiceMessagePhoneNumber == "+000000000000") {
                    UIHelper.Popup(PopupMessagesRes.VoiceMessageNumberShouldBeChanged, PopupMessagesRes.Title_Attention, MessageBoxButton.OK, MessageBoxImage.Warning);
                    // Enable the entire UI
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        DisableMainGrid?.Invoke(this, false); // Enable MainGrid
                    });
                    return;
                }

                if (response.Data.PhoneNumbers.Count == 0)
                {
                    UIHelper.Popup(PopupMessagesRes.NoAnyAssignedNumber, PopupMessagesRes.Title_Attention, MessageBoxButton.OK, MessageBoxImage.Warning);
                    // Enable the entire UI
                    UIHelper.ToggleMainGridState(false, DisableMainGrid);
                    return;
                }

                Random rand = new Random();
                var counter = 0;

                foreach (var phone in response.Data.PhoneNumbers)
                {
                    string message = messages[rand.Next(messages.Count)].Content;

                    await WhatsappHelper.SendWhatsAppMessageAsync(phone.PhoneNumber, message, SendVoiceMessage);

                    //// Mark PhoneNumber as sent for user
                    string apiUrlMarkPhoneNumberAsSent = "/AssignedPhoneNumber/MarkPhoneNumberAsSent";

                    // Prepare the request payload
                    var request = new MarkPhoneNumberAsSentRequest
                    {
                        AssignedPhoneNumberId = phone.AssignedPhoneNumberId
                    };

                    // Send the POST request using the helper
                    var responseMarkPhoneNumberAsSent = await RequestHelper.PostAsync<MarkPhoneNumberAsSentRequest, OperationResult>(apiUrlMarkPhoneNumberAsSent, request);

                    if (!responseMarkPhoneNumberAsSent.Success)
                    {
                        UIHelper.Popup(PopupMessagesRes.ErrorWhileUpdatingMessageStatus, PopupMessagesRes.Title_ErrorOccured, MessageBoxButton.OK, MessageBoxImage.Error);
                        // Enable the entire UI
                        UIHelper.ToggleMainGridState(false, DisableMainGrid);
                        break;
                    }
                }
            }
            else
            {
                UIHelper.Popup(PopupMessagesRes.CantLoadNumbers, PopupMessagesRes.Title_ErrorOccured, MessageBoxButton.OK, MessageBoxImage.Error);
            }

            // Enable the entire UI
            UIHelper.ToggleMainGridState(false, DisableMainGrid);
        }
        private void SendVoiceMessageCheckBox_Toggle(object sender, RoutedEventArgs e)
        {
            SendVoiceMessage = SendVoiceMessageCheckBox.IsChecked.Value;
        }
        #endregion

        #region Permission Action Methods
        // Call this wherever the checkbox needs to be updated
        public void CanSendVoiceMessageState(bool isEnabled)
        {
            SendVoiceMessageCheckBox.IsEnabled = isEnabled;
        }
        #endregion
    }
}
