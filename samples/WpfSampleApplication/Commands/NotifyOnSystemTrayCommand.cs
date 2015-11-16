using System;
using Nyx.AppSupport;
using Nyx.AppSupport.AppServices;
using Nyx.AppSupport.Commands;
using Nyx.AppSupport.SystemTray;

namespace WpfSampleApplication.Commands
{
    public class NotifyOnSystemTrayCommand : BaseCommand<String>
    {
        private readonly IUserNotificationService _notificationService;

        public NotifyOnSystemTrayCommand(ISystemTrayService notificationService)
        {
            _notificationService = notificationService;
        }

        public override void Execute(string parameter)
        {
            _notificationService.ShowInformation(parameter, "Notification");
        }
    }
}