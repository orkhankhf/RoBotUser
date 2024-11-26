using NLog;
using NLogLogger = NLog.ILogger;
using System.Windows.Controls;
using System.Windows;
using Common.Resources;
using Common.Helpers;
using Entities.RequestModels;
using Entities.Models;

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
                    MessageBox.Show("Mesajları yükləmək mümkün olmadı.", "Xəta", MessageBoxButton.OK, MessageBoxImage.Error);
                    return new List<Message>();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Mesajları yükləyərkən bir xəta baş verdi.", "Xəta", MessageBoxButton.OK, MessageBoxImage.Error);
                return new List<Message>();
            }
        }

        #region TextBox And Button Events
        private async void SendMessagesBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // Disable the entire UI
            Application.Current.Dispatcher.Invoke(() =>
            {
                DisableMainGrid?.Invoke(this, true);
            });

            // Simulate API call to get phone numbers which not sent message
            string apiUrl = "/AssignedPhoneNumber/GetUnsentPhoneNumbers";
            var response = await RequestHelper.GetAsync<GetUnsentPhoneNumbersResponse>(apiUrl);

            if (response.Success)
            {
                var messages = await GetMessageTemplates();

                if (messages.Count == 0)
                {
                    MessageBox.Show("Mesaj şablonları mövcud deyil!", "Diqqət", MessageBoxButton.OK, MessageBoxImage.Warning);
                    // Enable the entire UI
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        DisableMainGrid?.Invoke(this, false); // Enable MainGrid
                    });
                    return;
                }

                Random rand = new Random();
                var counter = 0;

                foreach (var phone in response.Data.PhoneNumbers)
                {
                    string message = messages[rand.Next(messages.Count)].Content;

                    await WhatsappHelper.SendWhatsAppMessageAsync(phone, message, SendVoiceMessage);

                    //var turboAzSetHasMessageSentOperation = new TurboAzSetHasMessageSentOperation
                    //{
                    //    Id = ad.Id
                    //};

                    ////update HasMessageSent
                    //await _turboAzAdInfoService.TurboAzSetHasMessageSentAsync(turboAzSetHasMessageSentOperation);
                }
            }
            else
            {
                MessageBox.Show("Nömrələr yüklənmədi!", "Xəta", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            // Enable the entire UI
            Application.Current.Dispatcher.Invoke(() =>
            {
                DisableMainGrid?.Invoke(this, false); // Enable MainGrid
            });
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
