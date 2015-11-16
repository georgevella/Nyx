using System.Windows.Forms;
using Nyx.AppSupport.Dialogs.Impl;

namespace Nyx.AppSupport.Dialogs
{
    class SelectFolderDialogCommand : BaseFileDialogCommand, ISelectFolderDialogCommand
    {
        protected override DialogResult ExecuteShowDialog(object parameter)
        {
            DialogResult dr = DialogResult.None;

            using (var dialog = new OpenFileDialog())
            {
                dialog.CheckFileExists = false;
                dialog.CheckPathExists = true;
                dialog.Title = "Select Folder ...";
                dialog.Multiselect = false;
                dialog.FileName = "[Folder]";
                dialog.Filter = "Folders only|*.FOLDER";


                switch (dr = dialog.ShowDialog())
                {
                    case DialogResult.OK:
                        Path = System.IO.Path.GetDirectoryName(dialog.FileName);
                        break;
                }
            }

            return dr;
        }
    }
}