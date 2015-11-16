using System.Windows.Forms;
using Nyx.AppSupport.Dialogs.Impl;

namespace Nyx.AppSupport.Dialogs
{
    /// <summary>
    /// </summary>
    internal class SaveFileDialogCommand : BaseFileDialogCommand, ISaveFileDialogCommand
    {
        protected override DialogResult ExecuteShowDialog(object parameter)
        {
            DialogResult dr;

            using (var d = new SaveFileDialog())
            {
                d.FileName = Path;
                d.Filter = Filter;
                d.ShowHelp = EnableHelpButton;
                d.Title = Title;
                d.DefaultExt = DefaultExtension;
                d.AddExtension = AddExtension;

                dr = d.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    Path = d.FileName;
                    OnPropertyChanged("Path");
                }
            }

            return dr;
        }
    }
}