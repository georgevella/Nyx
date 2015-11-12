using Nyx.AppSupport.Wpf.Commands;
using Nyx.AppSupport.Wpf.Dialogs;
using Nyx.Presentation;
using WpfSampleApplication.Commands;

namespace WpfSampleApplication.ViewModels
{
    public class MainViewModel : ViewModel<MainViewModel>
    {
        public ExitAppCommand ExitAppCommand { get; }
        public IOpenFileDialogCommand OpenFileCommand { get; }

        public MainViewModel(ISaveFileDialogCommand saveFileDialogCommand, IOpenFileDialogCommand openFileDialogCommand, ExitAppCommand exitApp)
        {
            ExitAppCommand = exitApp;
            OpenFileCommand = openFileDialogCommand;
            SaveFileCommand = saveFileDialogCommand;
        }

        public ISaveFileDialogCommand SaveFileCommand { get; }
    }
}