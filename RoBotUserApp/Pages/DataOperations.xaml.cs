using Common.Resources;
using NLog;
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
        }

        private async Task FillUITextsAsync()
        {
            LoadNewNumbersBtn.Content = UIRes.DataOperations_LoadNewNumbersBtn;
        }


        #region TextBox And Button Events
        private void LoadNewNumbersBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }
        #endregion
    }
}
