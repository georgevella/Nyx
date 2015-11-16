using System.Windows;
using Nyx.AppSupport;
using Nyx.AppSupport.Commands;

namespace WpfSampleApplication.Commands
{
    public class ExitAppCommand : BaseCommand
    {
        private readonly IApplicationServices _app;
        private readonly IUserNotificationService _notificationService;

        public ExitAppCommand(IApplicationServices app, IUserNotificationService notificationService)
        {
            _app = app;
            _notificationService = notificationService;
        }

        public override void Execute()
        {
            if (_notificationService.AskConfirmation("Are you sure you want to exit?", "Exit Confirmation"))
                _app.Exit();
        }

        public override bool CanExecute()
        {
            return true;
        }
    }
}