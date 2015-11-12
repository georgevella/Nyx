using System.Windows.Forms;

namespace Nyx.AppSupport.Wpf.AppServices
{
    public class SelectFolderService : ISelectFolderService
    {
        public bool Execute()
        {
            var bResult = false;
            var dialog = new OpenFileDialog();
            
            using (dialog)
            {
                dialog.CheckFileExists = false;
                dialog.CheckPathExists = true;
                dialog.Title = "Select Folder ...";
                dialog.Multiselect = false;
                dialog.FileName = "[Folder]";
                dialog.Filter = "Folders only|*.FOLDER";

                switch (dialog.ShowDialog())
                {
                    case DialogResult.OK:
                        bResult = true;
                        Path = System.IO.Path.GetDirectoryName(dialog.FileName);
                        break;
                }
            }

            return bResult;
        }

        public string Path
        {
            get; set;
        }

        public string Title
        {
            get; set;
        }
    }
}