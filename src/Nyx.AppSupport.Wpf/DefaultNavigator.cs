using System.Reflection;
using System.Windows;
using Nyx.Composition;
using Nyx.Presentation;
using Nyx.Presentation.Attributes;

namespace Nyx.AppSupport
{
    public class DefaultNavigator : INavigator
    {
        private readonly IContainer _container;
        private readonly IViewResolver _viewResolver;

        public DefaultNavigator(IContainer container, IViewResolver viewResolver)
        {
            _container = container;
            _viewResolver = viewResolver;
        }

        public TViewModel NavigateTo<TViewModel>() where TViewModel : IViewModel
        {
            var showModal = typeof(TViewModel).GetCustomAttribute<DialogAttribute>() != null;

            return NavigateTo<TViewModel>(showModal);
        }

        public TViewModel NavigateTo<TViewModel>(bool showModal) where TViewModel : IViewModel
        {
            var viewType = _viewResolver.ResolveViewTypeFor<TViewModel>();
            var view = (FrameworkElement)_container.Get(viewType);
            var viewModel = _container.Get<TViewModel>();

            view.DataContext = viewModel;

            var windowView = view as Window;
            if (windowView == null)
            {
                return viewModel;
            }

            var mainWindow = Application.Current.MainWindow;
            if (mainWindow != null && !Equals(mainWindow, windowView))
                windowView.Owner = mainWindow;

            if (showModal)
            {
                windowView.ShowDialog();
            }
            else
            {
                windowView.Show();
            }

            return viewModel;
        }
    }
}