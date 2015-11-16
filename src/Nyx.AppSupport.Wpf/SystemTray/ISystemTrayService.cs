using System;
using System.Windows.Forms;

namespace Nyx.AppSupport.SystemTray
{
    public interface ISystemTrayService : IUserNotificationService
    {
        ISystemTrayServiceConfigurator Configure();
        void Show();
        void Hide();
    }

    class SystemTrayService : ISystemTrayService
    {
        private readonly NotifyIcon _systemTrayIcon = new NotifyIcon();

        public ISystemTrayServiceConfigurator Configure()
        {
            return new SystemTrayServiceConfigurator(_systemTrayIcon);
        }

        public void Show()
        {
            _systemTrayIcon.Visible = true;
        }

        public void Hide()
        {
            _systemTrayIcon.Visible = false;
        }

        public void ShowInformation(string message, string caption)
        {
            ShowSystrayTooltip(ToolTipIcon.Info, message, caption);
        }

        private void ShowSystrayTooltip(ToolTipIcon toolTipIcon, string message, string caption)
        {
            _systemTrayIcon.BalloonTipIcon = toolTipIcon;
            _systemTrayIcon.BalloonTipText = message;
            _systemTrayIcon.BalloonTipTitle = caption;
            _systemTrayIcon.ShowBalloonTip(10 * 1000);
        }

        public void ShowWarning(string message, string caption)
        {
            ShowSystrayTooltip(ToolTipIcon.Warning, message, caption);
        }

        public void ShowError(string message, string caption)
        {
            ShowSystrayTooltip(ToolTipIcon.Error, message, caption);
        }

        public bool AskConfirmation(string message, string caption)
        {
            throw new NotImplementedException();
        }
    }
}