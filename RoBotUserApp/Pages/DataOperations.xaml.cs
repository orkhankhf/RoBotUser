using Common.Helpers;
using Common;
using Common.Resources;
using Entities.RequestModels;
using NLog;
using System.Windows;
using NLogLogger = NLog.ILogger;
using UserControl = System.Windows.Controls.UserControl;
using Entities.Models;

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
        }

        private async Task FillUITextsAsync()
        {
            LoadNewNumbersBtn.Content = UIRes.DataOperations_LoadNewNumbersBtn;
        }


        #region TextBox And Button Events
        private async void LoadNewNumbersBtn_Click(object sender, RoutedEventArgs e)
        {
            string apiUrl = $"/Data/GetPhoneNumbersByToken/{UserSettings.Token}";

            var res = await RequestHelper.GetAsync<GetPhoneNumbersByTokenResponse>(apiUrl);

            if (res.Success)
            {
                if (res.Data.RemainingWaitTime.HasValue) //user need to wait 1 or 24hour to refresh numbers
                {
                    var remainingTime = res.Data.RemainingWaitTime.Value;
                    TimeSpan truncatedRemainingTime = new TimeSpan(remainingTime.Hours, remainingTime.Minutes, remainingTime.Seconds);
                    MessageBox.Show(string.Format(UIRes.DataOperations_RemainingTimeLimitIssue, truncatedRemainingTime), UIRes.General_Attention, MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    var phoneNumberList = res.Data.PhoneNumbers.Select(p => new PhoneNumber
                    {
                        FormattedNumber = p.FormattedNumber,
                        PriceOfProduct = p.PriceOfProduct,
                        Currency = p.Currency,
                        City = p.City
                    }).ToList();

                    MessageBox.Show(string.Format(UIRes.DataOperations_NewNumbersSaved), UIRes.General_Attention, MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show(res.ErrorMessage, UIRes.General_ErrorOccured, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion
    }
}
