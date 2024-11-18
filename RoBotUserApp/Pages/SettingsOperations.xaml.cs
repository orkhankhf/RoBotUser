using NLog;
using NLogLogger = NLog.ILogger;
using System.Windows.Controls;

namespace RoBotUserApp.Pages
{
    public partial class SettingsOperations : UserControl
    {
        private static readonly NLogLogger Logger = LogManager.GetCurrentClassLogger();
        public event EventHandler<bool> DisableMainGrid;
        public SettingsOperations()
        {
            InitializeComponent();
            _ = FillUITextsAsync();
        }

        private async Task FillUITextsAsync()
        {

        }

        #region TextBox And Button Events

        #endregion
    }
}
