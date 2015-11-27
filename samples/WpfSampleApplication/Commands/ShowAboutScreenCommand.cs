using Nyx.AppSupport.Commands;
using Nyx.Presentation;
using WpfSampleApplication.ViewModels;

namespace WpfSampleApplication.Commands
{
    public class ShowAboutScreenCommand : BaseCommand
    {
        private readonly INavigator _navigator;

        public ShowAboutScreenCommand(INavigator navigator)
        {
            _navigator = navigator;
        }

        public override void Execute()
        {
            _navigator.NavigateTo<AboutViewModel>();
        }

        public override bool CanExecute()
        {
            throw new System.NotImplementedException();
        }
    }
}