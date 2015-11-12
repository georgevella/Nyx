using System.Windows.Forms;

namespace Nyx.AppSupport.Wpf.AppServices
{
    public interface IMessageBoxService : IApplicationService
    {
        MessageBoxButtons Buttons { get; set; }
        string Caption { get; set; }
        MessageBoxIcon Icon { get; set; }
        string Message { get; set; }
        DialogResult Result { get; }
    }
}