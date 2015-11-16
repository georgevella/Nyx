namespace Nyx.AppSupport.Dialogs.Impl
{
    internal abstract class BaseFileDialogCommand : BaseDialogCommand
    {
        private string _path;

        public string Path
        {
            get { return _path; }
            set
            {
                _path = value;
                OnPropertyChanged();
            }
        }

        public string Filter { get; set; }
        public string Title { get; set; }
        public string DefaultExtension { get; set; }
        public bool EnableHelpButton { get; set; }
        public bool AddExtension { get; set; }


    }
}