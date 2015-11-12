using Nyx.AppSupport.Wpf.Commands;
using Nyx.AppSupport.Wpf.Dialogs;
using Nyx.Presentation;

namespace WpfSampleApplication.ViewModels
{
    public class MainViewModel : ViewModel<MainViewModel>
    {
        public IOpenFileDialogCommand OpenFileCommand { get; }

        public MainViewModel(ISaveFileDialogCommand saveFileDialogCommand, IOpenFileDialogCommand openFileDialogCommand)
        {
            OpenFileCommand = openFileDialogCommand;
            SaveFileCommand = saveFileDialogCommand;
        }

        public ISaveFileDialogCommand SaveFileCommand { get; }
    }
}