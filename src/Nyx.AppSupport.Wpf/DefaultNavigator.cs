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

        public void NavigateTo<TViewModel>(TViewModel viewModel)
            where TViewModel : IViewModel
        {
            var showModal = typeof(TViewModel).GetCustomAttribute<DialogAttribute>() != null;
            NavigateTo(viewModel, showModal);
        }

        public void NavigateTo<TViewModel>(TViewModel viewModel, bool showModal)
            where TViewModel : IViewModel
        {
            var viewType = _viewResolver.ResolveViewTypeFor<TViewModel>();
            var view = (FrameworkElement)_container.Get(viewType);

            view.DataContext = viewModel;

            var windowView = view as Window;
            if (windowView == null)
            {
                // TODO: the fuck we do when viewmodel is not a window?
                return;
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
        }

        public TViewModel NavigateTo<TViewModel>() where TViewModel : IViewModel
        {
            var showModal = typeof(TViewModel).GetCustomAttribute<DialogAttribute>() != null;

            return NavigateTo<TViewModel>(showModal);
        }

        public TViewModel NavigateTo<TViewModel>(bool showModal) where TViewModel : IViewModel
        {
            var viewModel = _container.Get<TViewModel>();
            NavigateTo(viewModel, showModal);
            return viewModel;
        }
    }
}