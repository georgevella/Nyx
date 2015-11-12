using System.Windows.Forms;

namespace Nyx.AppSupport.Wpf.Dialogs
{
    /// <summary>
    /// </summary>
    public class SaveFileDialog : BaseFileDialog
    {
        protected override DialogResult ExecuteShowDialog(object parameter)
        {
            DialogResult dr;

            using (var d = new System.Windows.Forms.SaveFileDialog())
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