using Common.Helpers;
using Common.Resources;
using Entities.RequestModels;
using NLog;
using System.Windows;
using NLogLogger = NLog.ILogger;
using UserControl = System.Windows.Controls.UserControl;

namespace RoBotUserApp.Pages
{
    public partial class DataOperations : UserControl
    {
        private static readonly NLogLogger Logger = LogManager.GetCurrentClassLogger();
        public event EventHandler<bool> DisableMainGrid;
        public static bool SendVoiceMessage { get; set; }

        public DataOperations()
        {
            InitializeComponent();
            _ = FillUITextsAsync();
            _ = LoadStatisticsAsync();
        }

        private async Task FillUITextsAsync()
        {
            AssignNewNumbersBtn.Content = UIRes.DataOperations_AssignNewNumbersBtn;
        }

        private async Task LoadStatisticsAsync()
        {
            try
            {
                // Simulate API call to get statistics
                string apiUrl = "/AssignedPhoneNumber/GetAssignedPhoneNumberStatistics";
                var response = await RequestHelper.GetAsync<GetAssignedPhoneNumberStatisticsResponse>(apiUrl);

                if (response.Success)
                {
                    // Update statistics on the UI
                    TotalAssignedNumbersCountText.Text = string.Format(UIRes.DataOperations_TotalAssignedNumbersCountText, response.Data.WaitingForSendingMessageCount + response.Data.SentMessageNumberCount);
                    WaitingForSendingMessageCountText.Text = string.Format(UIRes.DataOperations_WaitingForSendingMessageCountText, response.Data.WaitingForSendingMessageCount);
                    SentMessageNumberCountText.Text = string.Format(UIRes.DataOperations_SentMessageNumberCountText, response.Data.SentMessageNumberCount);
                    LimitPerRequestText.Text = string.Format(UIRes.DataOperations_LimitPerRequestText, response.Data.LimitPerRequest);

                    if (response.Data.LastAssignTime.HasValue)
                    {
                        LastAssignTimeText.Text = string.Format(UIRes.DataOperations_LastAssignTimeText, DateTimeHelper.LocalDate(response.Data.LastAssignTime.Value));
                    }
                }
                else
                {
                    MessageBox.Show("Statistik məlumatlar yüklənə bilmədi.", "Xəta", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch
            {
                MessageBox.Show("Statistik məlumatları yükləyərkən xəta baş verdi.", "Xəta", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        #region TextBox And Button Events
        private async void AssignNewNumbersBtn_Click(object sender, RoutedEventArgs e)
        {
            DisableMainGrid?.Invoke(this, true); // Disable UI during API call

            try
            {
                // Define the API endpoint
                string apiUrl = "/AssignedPhoneNumber/AssignPhoneNumbers";

                // Prepare the request payload
                var request = new AssignPhoneNumbersRequest
                {

                };

                // Send the POST request using the helper
                var response = await RequestHelper.PostAsync<AssignPhoneNumbersRequest, AssignPhoneNumbersResponse>(apiUrl, request);

                // Handle the response
                if (response.Success)
                {
                    if (response.Data.RemainingWaitTime.HasValue) //user need to wait 1 or 24hour to refresh numbers
                    {
                        var remainingTime = response.Data.RemainingWaitTime.Value;
                        TimeSpan truncatedRemainingTime = new TimeSpan(remainingTime.Hours, remainingTime.Minutes, remainingTime.Seconds);
                        MessageBox.Show(string.Format(UIRes.DataOperations_RemainingTimeLimitIssue, truncatedRemainingTime), UIRes.General_Attention, MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    else
                    {
                        MessageBox.Show("Yeni nömrələr uğurla təyin edildi.", "Məlumat", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    MessageBox.Show($"Nömrələri təyin edərkən xəta baş verdi: {response.ErrorMessage}", "Xəta", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nömrələr təyin olunarkən xəta baş verdi!", "Xəta", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            DisableMainGrid?.Invoke(this, false); // Re-enable UI after API call
        }
        #endregion

        private async void RefreshStatisticsBtn_Click(object sender, RoutedEventArgs e)
        {
            await LoadStatisticsAsync();
        }
    }
}
