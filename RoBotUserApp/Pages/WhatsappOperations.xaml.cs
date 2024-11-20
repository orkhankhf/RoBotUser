using NLog;
using NLogLogger = NLog.ILogger;
using System.Windows.Controls;
using System.Windows;
using Common.Resources;
using Common;

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

        #region TextBox And Button Events
        private void SendMessagesBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {

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
