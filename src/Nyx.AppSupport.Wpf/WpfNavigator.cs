using System.Windows;
using Nyx.Composition;
using Nyx.Presentation;

namespace Nyx.AppSupport.Wpf
{
    public class WpfNavigator : INavigator
    {
        private readonly IContainer _container;
        private readonly IViewResolver _viewResolver;

        public WpfNavigator(IContainer container, IViewResolver viewResolver)
        {
            _container = container;
            _viewResolver = viewResolver;
        }

        public TViewModel NavigateTo<TViewModel>() where TViewModel : IViewModel
        {

            var viewType = _viewResolver.ResolveViewTypeFor<TViewModel>();
            var view = (FrameworkElement)_container.Get(viewType);
            var viewModel = _container.Get<TViewModel>();

            view.DataContext = viewModel;

            var windowView = view as Window;
            if (windowView != null)
            {
                var mainWindow = Application.Current.MainWindow;
                if (mainWindow != null && !Equals(mainWindow, windowView))
                    windowView.Owner = mainWindow;
                windowView.Show();
            }

            return viewModel;
        }
    }
}