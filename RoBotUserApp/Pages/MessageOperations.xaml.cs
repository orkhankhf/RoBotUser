using Common.Helpers;
using Entities.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Entities.RequestModels;
using RoBotUserApp.UiHelpers;
using Common.Resources;

namespace RoBotUserApp.Pages
{
    public partial class MessageOperations : UserControl
    {
        public event EventHandler<bool> DisableMainGrid;
        private const int MaxMessageLength = 750;

        public MessageOperations()
        {
            InitializeComponent();
            _ = LoadExistingMessageTemplatesAsync();
        }

        private async Task LoadExistingMessageTemplatesAsync()
        {
            UIHelper.ToggleMainGridState(true, DisableMainGrid); // Disable UI during API call

            try
            {
                string apiUrl = $"/Messages/User/";
                var response = await RequestHelper.GetAsync<List<Message>>(apiUrl);

                if (response.Success)
                {
                    foreach (var message in response.Data)
                    {
                        AddMessageTemplateToUI(message.Id, message.Content);
                    }
                }
                else
                {
                    UIHelper.Popup(PopupMessagesRes.CantLoadMessageTemplates, PopupMessagesRes.Title_ErrorOccured, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                UIHelper.Popup(PopupMessagesRes.CantLoadMessageTemplates, PopupMessagesRes.Title_ErrorOccured, MessageBoxButton.OK, MessageBoxImage.Error);
            }

            UIHelper.ToggleMainGridState(false, DisableMainGrid); // Re-enable UI after API call
        }

        private void AddMessageBtn_Click(object sender, RoutedEventArgs e)
        {
            // Add a new message template with no ID (ID = 0 for new messages)
            AddMessageTemplateToUI(0, string.Empty);
        }

        private void AddMessageTemplateToUI(int messageId, string messageContent)
        {
            var containerGrid = new Grid
            {
                Margin = new Thickness(10),
                HorizontalAlignment = HorizontalAlignment.Stretch
            };

            containerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) }); // TextBox width
            containerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }); // Buttons width

            var messageTextBox = new TextBox
            {
                Text = messageContent,
                Width = double.NaN,
                Height = 100,
                TextWrapping = TextWrapping.Wrap,
                MaxLength = MaxMessageLength,
                AcceptsReturn = true,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = new Thickness(5),
                Tag = messageId // Use the Tag to store the message ID
            };
            Grid.SetColumn(messageTextBox, 0);
            containerGrid.Children.Add(messageTextBox);

            var buttonPanel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(5)
            };

            var saveButton = new Button
            {
                Content = "Yadda saxla",
                Width = 100,
                Margin = new Thickness(0, 5, 0, 5),
                Tag = messageTextBox // Reference the associated TextBox
            };
            saveButton.Click += SaveMessageButton_Click;

            var removeButton = new Button
            {
                Content = "Sil",
                Width = 100,
                Background = Brushes.IndianRed,
                Foreground = Brushes.White,
                Margin = new Thickness(0, 5, 0, 5),
                Tag = messageId // Store the message ID in the Tag
            };
            removeButton.Click += RemoveMessageButton_Click;

            buttonPanel.Children.Add(saveButton);
            buttonPanel.Children.Add(removeButton);

            Grid.SetColumn(buttonPanel, 1);
            containerGrid.Children.Add(buttonPanel);

            MessageTemplatesPanel.Children.Insert(0, containerGrid);
        }

        private async void SaveMessageButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var textBox = button.Tag as TextBox;

            if (textBox != null)
            {
                var messageContent = textBox.Text.Trim();
                var messageId = (int)(textBox.Tag);

                if (string.IsNullOrWhiteSpace(messageContent))
                {
                    UIHelper.Popup(PopupMessagesRes.MessageCantBeEmpty, PopupMessagesRes.Title_Attention, MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (messageContent.Length > MaxMessageLength)
                {
                    UIHelper.Popup(string.Format(PopupMessagesRes.MessageMaxLengthExceeded, MaxMessageLength), PopupMessagesRes.Title_Attention, MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                UIHelper.ToggleMainGridState(true, DisableMainGrid); // Disable UI during API call

                try
                {
                    if (messageId == 0)
                    {
                        // Add new message via API
                        string apiUrl = "/Messages";
                        var request = new AddOrUpdateMessageRequest
                        {
                            Content = messageContent
                        };

                        var response = await RequestHelper.PostAsync<AddOrUpdateMessageRequest, Message>(apiUrl, request);

                        if (response.Success)
                        {
                            textBox.Tag = response.Data.Id; // Update the TextBox Tag with the new message ID
                            UIHelper.Popup(PopupMessagesRes.AddedNewMessage, PopupMessagesRes.Title_Info, MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            UIHelper.Popup(PopupMessagesRes.ErrorWhileAddingMessage, PopupMessagesRes.Title_ErrorOccured, MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        // Update existing message via API
                        string apiUrl = $"/Messages/{messageId}";
                        var request = new AddOrUpdateMessageRequest
                        {
                            Content = messageContent
                        };

                        var response = await RequestHelper.PutAsync<AddOrUpdateMessageRequest, OperationResult>(apiUrl, request);

                        if (response.Success && response.Data.IsSuccessful)
                        {
                            UIHelper.Popup(PopupMessagesRes.MessageUpdated, PopupMessagesRes.Title_Info, MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            UIHelper.Popup(PopupMessagesRes.ErrorWhileUpdatingMessage, PopupMessagesRes.Title_ErrorOccured, MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    UIHelper.Popup(PopupMessagesRes.ErrorWhileUpdatingMessage, PopupMessagesRes.Title_ErrorOccured, MessageBoxButton.OK, MessageBoxImage.Error);
                }

                UIHelper.ToggleMainGridState(false, DisableMainGrid); // Re-enable UI after API call
            }
        }

        private async void RemoveMessageButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            int messageId = (int)button.Tag;

            if (messageId == 0)
            {
                // Remove from UI only for unsaved messages
                RemoveMessageTemplateFromUI(button);
                return;
            }

            UIHelper.ToggleMainGridState(true, DisableMainGrid); // Disable UI during API call

            try
            {
                string apiUrl = $"/Messages/{messageId}";
                var response = await RequestHelper.DeleteAsync<OperationResult>(apiUrl);

                if (response.Success && response.Data.IsSuccessful)
                {
                    RemoveMessageTemplateFromUI(button);
                    UIHelper.Popup(PopupMessagesRes.MessageDeleted, PopupMessagesRes.Title_Info, MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    UIHelper.Popup(PopupMessagesRes.ErrorWhileDeletingMessage, PopupMessagesRes.Title_ErrorOccured, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                UIHelper.Popup(PopupMessagesRes.ErrorWhileDeletingMessage, PopupMessagesRes.Title_ErrorOccured, MessageBoxButton.OK, MessageBoxImage.Error);
            }

            UIHelper.ToggleMainGridState(false, DisableMainGrid); // Re-enable UI after API call
        }

        private void RemoveMessageTemplateFromUI(Button button)
        {
            var buttonPanel = button.Parent as StackPanel;
            var container = buttonPanel.Parent as Grid;

            if (container != null)
            {
                MessageTemplatesPanel.Children.Remove(container);
            }
        }
    }
}
