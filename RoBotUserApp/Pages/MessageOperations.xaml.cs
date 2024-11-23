using Common.Helpers;
using Common;
using Entities.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Entities.RequestModels;

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
            DisableMainGrid?.Invoke(this, true); // Disable UI during API call

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
                    MessageBox.Show("Mesajları yükləmək mümkün olmadı.", "Xəta", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Mesajları yükləyərkən bir xəta baş verdi.", "Xəta", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            DisableMainGrid?.Invoke(this, false); // Re-enable UI after API call
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
                    MessageBox.Show("Mesaj boş ola bilməz.", "Diqqət", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (messageContent.Length > MaxMessageLength)
                {
                    MessageBox.Show($"Mesaj {MaxMessageLength} simvoldan çox ola bilməz.", "Diqqət", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                DisableMainGrid?.Invoke(this, true); // Disable UI during API call

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
                            MessageBox.Show("Yeni mesaj əlavə edildi.", "Məlumat", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show("Mesaj əlavə edilə bilmədi.", "Xəta", MessageBoxButton.OK, MessageBoxImage.Error);
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
                            MessageBox.Show("Mesaj yeniləndi.", "Məlumat", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show("Mesaj yenilənə bilmədi.", "Xəta", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Mesaj saxlanarkən bir xəta baş verdi.", "Xəta", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                DisableMainGrid?.Invoke(this, false); // Re-enable UI after API call
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

            DisableMainGrid?.Invoke(this, true); // Disable UI during API call

            try
            {
                string apiUrl = $"/Messages/{messageId}";
                var response = await RequestHelper.DeleteAsync<OperationResult>(apiUrl);

                if (response.Success && response.Data.IsSuccessful)
                {
                    RemoveMessageTemplateFromUI(button);
                    MessageBox.Show("Mesaj silindi.", "Məlumat", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Mesaj silinə bilmədi.", "Xəta", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Mesaj silinərkən bir xəta baş verdi.", "Xəta", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            DisableMainGrid?.Invoke(this, false); // Re-enable UI after API call
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
