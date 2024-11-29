using Common.Helpers;
using Common.Resources;
using Entities.Enums;
using Entities.RequestModels;
using NLog;
using RoBotUserApp.UiHelpers;
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
            _ = PopulateFilterOptionsAsync();
            _ = FillUITextsAsync();
            _ = LoadStatisticsAsync();
        }

        private async Task FillUITextsAsync()
        {
            AssignNewNumbersBtn.Content = UIRes.DataOperations_AssignNewNumbersBtn;
            SearchText.Text = UIRes.DataOperations_SearchText;
            SelectedCitiesText.Text = UIRes.DataOperations_SelectedCitiesText;
            SelectedCategoriesText.Text = UIRes.DataOperations_SelectedCategoriesText;
            PriceRangeText.Text = UIRes.DataOperations_PriceRangeText;
            GetFilteredCountBtn.Content = UIRes.DataOperations_ShowFilteredResultText;
            FilteredCountText.Text = string.Format(UIRes.DataOperations_FilteredCountText, 0);
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
                    UIHelper.Popup(PopupMessagesRes.StatisticsNotLoaded, PopupMessagesRes.Title_ErrorOccured, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch
            {
                UIHelper.Popup(PopupMessagesRes.StatisticsNotLoaded, PopupMessagesRes.Title_ErrorOccured, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #region Filter Section
        private async Task PopulateFilterOptionsAsync()
        {
            // Populate CitiesListBox dynamically with enum descriptions
            CitiesListBox.ItemsSource = EnumHelper.GetEnumDescriptionsForMultiSelectElements<CityEnum>();

            // Populate CategoriesListBox dynamically with enum descriptions
            CategoriesListBox.ItemsSource = EnumHelper.GetEnumDescriptionsForMultiSelectElements<CategoryEnum>();
        }

        private async void FilterBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UIHelper.ToggleMainGridState(true, DisableMainGrid);

                // Dynamically get selected cities from the CitiesListBox
                var selectedCities = CitiesListBox.SelectedItems
                    .Cast<string>()
                    .Select(city => (int)EnumHelper.MapDescriptionToEnum<CityEnum>(city))
                    .ToList();

                // Dynamically get selected categories from the CategoriesListBox
                var selectedCategories = CategoriesListBox.SelectedItems
                    .Cast<string>()
                    .Select(category => (int)EnumHelper.MapDescriptionToEnum<CategoryEnum>(category))
                    .ToList();

                // Parse PriceFrom and PriceTo dynamically from TextBoxes
                decimal? priceFrom = string.IsNullOrWhiteSpace(PriceFromTextBox.Text)
                    ? null
                    : decimal.Parse(PriceFromTextBox.Text);

                decimal? priceTo = string.IsNullOrWhiteSpace(PriceToTextBox.Text)
                    ? null
                    : decimal.Parse(PriceToTextBox.Text);

                // Dynamically build the request object
                var request = new GetFilteredPhoneNumbersRequest
                {
                    Cities = selectedCities,
                    Categories = selectedCategories,
                    PriceFrom = priceFrom,
                    PriceTo = priceTo
                };

                // Call the API
                string apiUrl = "/AssignedPhoneNumber/GetFilteredPhoneNumbersCount";
                var response = await RequestHelper.GetFilteredAsync<GetFilteredPhoneNumbersResponse>(apiUrl, request);

                if (response.Success)
                {
                    // Update the filtered count on the UI
                    FilteredCountText.Text = $"Tapıldı: {response.Data.Count}";
                }
                else
                {
                    UIHelper.Popup("Error while filtering numbers.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                UIHelper.Popup($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                UIHelper.ToggleMainGridState(false, DisableMainGrid);
            }
        }
        #endregion


        #region TextBox And Button Events
        private async void AssignNewNumbersBtn_Click(object sender, RoutedEventArgs e)
        {
            UIHelper.ToggleMainGridState(true, DisableMainGrid); // Disable UI during API call

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
                        UIHelper.Popup(string.Format(PopupMessagesRes.RemainingTimeLimitIssue, truncatedRemainingTime), PopupMessagesRes.Title_Attention, MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    else
                    {
                        UIHelper.Popup(PopupMessagesRes.NumbersAssigned, PopupMessagesRes.Title_Info, MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    UIHelper.Popup(string.Format(PopupMessagesRes.ErrorWhileAssignNumbers, response.ErrorMessage), PopupMessagesRes.Title_ErrorOccured, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                UIHelper.Popup(string.Format(PopupMessagesRes.ErrorWhileAssignNumbers, ex.Message), PopupMessagesRes.Title_ErrorOccured, MessageBoxButton.OK, MessageBoxImage.Error);
            }

            UIHelper.ToggleMainGridState(false, DisableMainGrid); // Re-enable UI after API call
        }
        #endregion

        private async void RefreshStatisticsBtn_Click(object sender, RoutedEventArgs e)
        {
            await LoadStatisticsAsync();
        }
    }
}
