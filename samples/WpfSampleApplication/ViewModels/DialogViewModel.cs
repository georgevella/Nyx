using Nyx.Presentation;
using Nyx.Presentation.Attributes;

namespace WpfSampleApplication.ViewModels
{
    [Dialog]
    public class DialogViewModel : ViewModel<DialogViewModel>
    {
        public DialogViewModel(IUserInterfaceThread userInterfaceThread) : base(userInterfaceThread) { }
    }
}