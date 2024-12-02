using Common;
using Common.Helpers;
using Common.Resources;
using Entities.Enums;
using Entities.Models;
using Entities.RequestModels;
using RoBotUserApp.Pages;
using RoBotUserApp.UiHelpers;
using System.Windows;

namespace RoBotUserApp
{
    public partial class MainWindow : Window
    {
        private WhatsappOperations _whatsappOperationsPage;
        private MessageOperations _messageOperationsPage;
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
            ExitAppBtn.Content = UIRes.ExitAppBtn;
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
            _whatsappOperationsPage = new WhatsappOperations();

            _whatsappOperationsPage.DisableMainGrid += DisableMainGrid;
            WhatsappOperationsContent.Content = _whatsappOperationsPage;
            WhatsappOperationsTab.Visibility = Visibility.Visible;

            //permission state changer for send voice messages
            _whatsappOperationsPage.CanSendVoiceMessage += isEnabled =>
            {
                _whatsappOperationsPage.CanSendVoiceMessageState(isEnabled);
            };
        }

        private void LoadSettingsOperationsPage()
        {
            // Load Data Operations Page
            SettingsOperations settingsOperationsPage = new SettingsOperations();
            settingsOperationsPage.DisableMainGrid += DisableMainGrid;
            SettingsOperationsContent.Content = settingsOperationsPage;
            SettingsOperationsTab.Visibility = Visibility.Visible;
        }

        private void LoadMessageOperationsPage()
        {
            // Load Data Operations Page
            _messageOperationsPage = new MessageOperations();

            _messageOperationsPage.DisableMainGrid += DisableMainGrid;
            MessageOperationsContent.Content = _messageOperationsPage;
            MessageOperationsTab.Visibility = Visibility.Visible;

            //permission state changer for add multiple message templates
            _messageOperationsPage.CanSendDifferentTextMessages += isEnabled =>
            {
                _messageOperationsPage.CanSendDifferentTextMessagesState(isEnabled);
            };
        }

        private void LoadAllPages()
        {
            // Hide all tabs by default
            HideAllTabs();

            LoadDataOperationsPage();
            LoadWhatsappOperationsPage();
            LoadSettingsOperationsPage();
            LoadMessageOperationsPage();

            // Show the tab control after permissions are applied
            LoginStackPanel.Visibility = Visibility.Hidden;
            MainTabControl.Visibility = Visibility.Visible;
        }

        private void HideAllTabs()
        {
            // Hide all tabs initially
            DataOperationsTab.Visibility = Visibility.Collapsed;
            WhatsappOperationsTab.Visibility = Visibility.Collapsed;
            SettingsOperationsTab.Visibility = Visibility.Collapsed;
        }
        #endregion

        
        private void ApplyRolePermissions(List<Permission> permissions)
        {
            //can send voice message
            if (permissions.FirstOrDefault(m => m.PermissionFor == PermissionEnum.CanSendVoiceMessage && m.Value == 1) == null)
            {
                _whatsappOperationsPage?.CanSendVoiceMessageState(false); // Disable checkbox
            }

            //Can send different text messages
            if (permissions.FirstOrDefault(m=>m.PermissionFor == PermissionEnum.CanSendDifferentTextMessages && m.Value == 1) == null)
            {
                _messageOperationsPage.CanSendDifferentTextMessagesState(false);
            }
        }

        #region TextBox And Button Events
        private async void LoginWithTokenBtn_Click(object sender, RoutedEventArgs e)
        {
            string token = TokenTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(token))
            {
                UIHelper.Popup(PopupMessagesRes.General_PleaseEnterValidToken, PopupMessagesRes.Title_Attention, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string apiUrl = $"/UserToken/AuthByToken/{token}";

            var res = await RequestHelper.GetAsync<AuthByTokenResponse>(apiUrl);
            
            if (res.Success)
            {
                //assign user settings
                UserSettings.Token = token;
                UserSettings.UserRole = res.Data.Role;
                UserSettings.Permissions = res.Data.Permissions;
                UserSettings.UserAppSettings = res.Data.UserAppSettings;

                LoadAllPages();

                //Apply role permissions
                ApplyRolePermissions(UserSettings.Permissions);
            }
            else
            {
                UIHelper.Popup(res.ErrorMessage, PopupMessagesRes.Title_ErrorOccured, MessageBoxButton.OK, MessageBoxImage.Error);
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