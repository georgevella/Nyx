using System.Windows.Forms;

namespace Nyx.AppSupport.Wpf.AppServices
{
    public class MessageBoxService : IApplicationService, IMessageBoxService
    {
        public string Message { get; set; }
        public string Caption { get; set; }
        //public MessageBoxButton Buttons { get; set; }
        public MessageBoxButtons Buttons { get; set; }
        //public MessageBoxImage Icon { get; set; }
        public MessageBoxIcon Icon { get; set; }
        //public MessageBoxResult Result { get; private set; }
        public DialogResult Result { get; private set; }

        public bool Execute()
        {
            Result = MessageBox.Show(
                Message,
                Caption,
                Buttons,
                Icon);

            switch (Result)
            {
                case DialogResult.OK:
                case DialogResult.Yes:
                    return true;
                default:
                    return false;
            }
        }

    }
}