using Common;
using Common.Helpers;
using Common.Resources;
using Entities.Enums;
using Entities.RequestModels;
using Microsoft.Extensions.DependencyInjection;
using RoBotApp;
using RoBotUserApp.Pages;
using System.Windows;

namespace RoBotUserApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.Manual;
            this.Left = 50; // Horizontal position
            this.Top = 20;  // Vertical position

            FillUITexts();
        }

        private void FillUITexts()
        {
            DataOperationsTab.Header = UIRes.MainWindow_DataOperationsTab;
            WhatsappOperationsTab.Header = UIRes.MainWindow_WhatsappOperationsTab;
            SettingsOperationsTab.Header = UIRes.MainWindow_SettingsOperationsTab;
        }

        private void DisableMainGrid(object sender, bool disable)
        {
            MainGrid.IsEnabled = !disable; // Disable or enable MainGrid based on the event
        }

        #region Load/Hide Pages
        private void LoadDataOperationsPage()
        {
            // Load Data Operations Page
            DataOperations dataOperationsPage = new DataOperations();
            dataOperationsPage.DisableMainGrid += DisableMainGrid;
            DataOperationsContent.Content = dataOperationsPage;
            DataOperationsTab.Visibility = Visibility.Visible;
        }

        private void LoadWhatsappOperationsPage()
        {
            // Load Data Operations Page
            WhatsappOperations whatsappOperationsPage = new WhatsappOperations();
            whatsappOperationsPage.DisableMainGrid += DisableMainGrid;
            WhatsappOperationsContent.Content = whatsappOperationsPage;
            WhatsappOperationsTab.Visibility = Visibility.Visible;
        }

        private void LoadSettingsOperationsPage()
        {
            // Load Data Operations Page
            SettingsOperations settingsOperationsPage = new SettingsOperations();
            settingsOperationsPage.DisableMainGrid += DisableMainGrid;
            SettingsOperationsContent.Content = settingsOperationsPage;
            SettingsOperationsTab.Visibility = Visibility.Visible;
        }

        private void HideAllTabs()
        {
            // Hide all tabs initially
            DataOperationsTab.Visibility = Visibility.Collapsed;
            WhatsappOperationsTab.Visibility = Visibility.Collapsed;
            SettingsOperationsTab.Visibility = Visibility.Collapsed;
        }
        #endregion

        
        private void ApplyRolePermissions(RoleEnum role)
        {
            HideAllTabs(); // Hide all tabs by default

            switch (role)
            {
                case RoleEnum.Standart:
                    LoadDataOperationsPage();
                    LoadWhatsappOperationsPage();
                    LoadSettingsOperationsPage();
                    break;
                case RoleEnum.Premium:
                    LoadDataOperationsPage();
                    LoadWhatsappOperationsPage();
                    LoadSettingsOperationsPage();
                    break;
                case RoleEnum.PremiumPlus:
                    LoadDataOperationsPage();
                    LoadWhatsappOperationsPage();
                    LoadSettingsOperationsPage();
                    break;
                case RoleEnum.Individual:
                    LoadDataOperationsPage();
                    LoadWhatsappOperationsPage();
                    LoadSettingsOperationsPage();
                    break;
            }

            LoginStackPanel.Visibility = Visibility.Hidden;
            MainTabControl.Visibility = Visibility.Visible; // Show the tab control after permissions are applied
        }

        #region TextBox And Button Events
        private async void LoginWithTokenBtn_Click(object sender, RoutedEventArgs e)
        {
            string token = TokenTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(token))
            {
                MessageBox.Show(UIRes.General_PleaseEnterValidToken, UIRes.General_Attention, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string apiUrl = $"/UserToken/GetRoleByToken/{token}";

            var res = await RequestHelper.GetAsync<GetRoleByTokenResponse>(apiUrl);
            
            if (res.Success)
            {
                //Apply role permissions
                ApplyRolePermissions(res.Data.Role);

                //assign user settings
                UserSettings.Token = token;
                UserSettings.UserRole = res.Data.Role;
            }
            else
            {
                MessageBox.Show(res.ErrorMessage, UIRes.General_ErrorOccured, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        private void RestartAppBtn_Click(object sender, RoutedEventArgs e)
        {
            // Restart the application
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }
    }
}