using Nyx.Messaging;
using Nyx.Presentation;
using Nyx.Presentation.Attributes;
using WpfSampleApplication.Messages;

namespace WpfSampleApplication.ViewModels
{
    [Dialog]
    public class AboutViewModel : ViewModel<AboutViewModel>, IMessageHandler<ShowAboutBoxMessage>
    {
        private readonly INavigator _navigator;

        public AboutViewModel(IUserInterfaceThread userInterfaceThread, INavigator navigator) : base(userInterfaceThread)
        {
            _navigator = navigator;
        }

        public void Handle(ShowAboutBoxMessage message)
        {
            _navigator.NavigateTo(this);
        }
    }
}