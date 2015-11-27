using Nyx.Presentation;
using Nyx.Presentation.Attributes;

namespace WpfSampleApplication.ViewModels
{
    [Dialog]
    public class AboutViewModel : ViewModel<AboutViewModel>
    {
        public AboutViewModel(IUserInterfaceThread userInterfaceThread) : base(userInterfaceThread)
        {
        }
    }
}