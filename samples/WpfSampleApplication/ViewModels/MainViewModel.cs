using System.Windows.Input;
using Nyx.AppSupport.Commands;
using Nyx.AppSupport.Dialogs;
using Nyx.AppSupport.SystemTray;
using Nyx.Presentation;
using WpfSampleApplication.Commands;

namespace WpfSampleApplication.ViewModels
{
    public class MainViewModel : ViewModel<MainViewModel>
    {
        private readonly ISystemTrayService _systemTray;
        public NotifyOnSystemTrayCommand SystemTrayNotificationCommand { get; }
        public ExitAppCommand ExitAppCommand { get; }
        public IOpenFileDialogCommand OpenFileCommand { get; }

        public MainViewModel(
            ISystemTrayService systemTray,
            ISaveFileDialogCommand saveFileDialogCommand,
            IOpenFileDialogCommand openFileDialogCommand,
            ExitAppCommand exitApp,
            NotifyOnSystemTrayCommand systemTrayNotificationCommand)
        {
            _systemTray = systemTray;
            SystemTrayNotificationCommand = systemTrayNotificationCommand;
            ExitAppCommand = exitApp;
            OpenFileCommand = openFileDialogCommand;
            SaveFileCommand = saveFileDialogCommand;

            ShowSystemTrayIconCommand = new DelegateCommand(() => _systemTray.Show());
            HideSystemTrayIconCommand = new DelegateCommand(() => _systemTray.Hide());
        }

        public ICommand HideSystemTrayIconCommand { get; }

        public ICommand ShowSystemTrayIconCommand { get; }

        public ISaveFileDialogCommand SaveFileCommand { get; }
    }
}