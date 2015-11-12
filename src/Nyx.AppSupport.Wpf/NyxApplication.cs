using System;
using System.Reflection;
using System.Windows;
using Nyx.Composition;
using Nyx.Presentation;
using Nyx.Presentation.Conventions;

namespace Nyx.AppSupport.Wpf
{
    class NyxApplication : INyxApplication
    {
        private readonly Application _app;
        private readonly IViewResolver _viewResolver;
        private readonly IContainerConfiguration _containerConfiguration;
        private ByNameViewModelDiscoveryConvention _convention;

        public NyxApplication(Application app, IViewResolver viewResolver, IContainerConfiguration containerConfiguration)
        {
            _app = app;
            _viewResolver = viewResolver;
            _containerConfiguration = containerConfiguration;
        }

        private void AutoDiscoverViewModelsImpl(Assembly assembly)
        {
            if (_convention == null)
            {
                _convention = new ByNameViewModelDiscoveryConvention();
            }

            _convention.FindAndRegisterMappings(_viewResolver, assembly);
        }


        public INyxApplication UsingDefaultConventions()
        {
            _convention = new ByNameViewModelDiscoveryConvention();
            return this;
        }

        public INyxApplication AutoDiscoverViewModels(Assembly assembly)
        {
            AutoDiscoverViewModelsImpl(assembly);
            return this;
        }

        public INyxApplication AutoDiscoverViewModels()
        {
            AutoDiscoverViewModelsImpl(_app.GetType().Assembly);
            return this;
        }

        public IServiceRegistration<TService> Register<TService>()
        {
            return _containerConfiguration.Register<TService>();
        }

        public IServiceRegistration Register(Type type)
        {
            return _containerConfiguration.Register(type);
        }
    }
}