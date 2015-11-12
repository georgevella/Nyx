using System.Windows.Forms;

namespace Nyx.AppSupport.Wpf.AppServices
{
    public class SelectFileService : ISelectFileService
    {
        private string[] _filenameList = new[] {string.Empty};
        public string FileName { get; set; }

        public string[] FileNameList
        {
            get { return _filenameList; }
            set { _filenameList = value; }
        }

        public SelectFileMode Mode { get; set; }
        public string DefaultExtension { get; set; }
        public string Filter { get; set; }
        public string Title { get; set; }

        public bool Execute()
        {
            var bResult = false;

            FileDialog dlg = null;
            switch (Mode)
            {
                case SelectFileMode.OpenFile:
                case SelectFileMode.OpenMultiFile:
                    dlg = new OpenFileDialog();

                    ((OpenFileDialog)dlg).Multiselect = Mode == SelectFileMode.OpenMultiFile;
                    dlg.CheckFileExists = true;
                    break;
                case SelectFileMode.SaveFile:
                    dlg = new SaveFileDialog();
                    break;
                default:
                    dlg = new OpenFileDialog();
                    break;
            }

            using (dlg)
            {
                dlg.Filter = this.Filter;
                dlg.FileName = this.FileName;
                dlg.Title = this.Title;
                dlg.DefaultExt = this.DefaultExtension;                

                switch (dlg.ShowDialog())
                {
                    case DialogResult.OK:
                        bResult = true;
                        FileName = dlg.FileName;
                        FileNameList = dlg.FileNames;
                        break;
                    default:
                        bResult = false;
                        break;
                }
            }

            return bResult;
        }


    }
}