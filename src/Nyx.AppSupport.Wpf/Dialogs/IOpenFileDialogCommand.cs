using System.Windows.Input;

namespace Nyx.AppSupport.Wpf.Dialogs
{
    public interface IOpenFileDialogCommand : ICommand
    {
        string Path { get; set; }
        string Filter { get; set; }
        string Title { get; set; }
        string DefaultExtension { get; set; }
        bool EnableHelpButton { get; set; }
        bool AddExtension { get; set; }

        bool EnableReadOnlyCheckbox { get; set; }
    }
}