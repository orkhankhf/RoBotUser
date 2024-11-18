using NLog;
using NLogLogger = NLog.ILogger;
using System.Windows.Controls;

namespace RoBotUserApp.Pages
{
    public partial class WhatsappOperations : UserControl
    {
        private static readonly NLogLogger Logger = LogManager.GetCurrentClassLogger();
        public event EventHandler<bool> DisableMainGrid;
        public WhatsappOperations()
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
