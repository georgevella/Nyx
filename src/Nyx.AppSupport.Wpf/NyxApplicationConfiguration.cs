using System;
using System.Reflection;
using System.Windows;
using Nyx.Composition;
using Nyx.Presentation;
using Nyx.Presentation.Conventions;

namespace Nyx.AppSupport.Wpf
{
    class NyxApplicationConfiguration : INyxApplicationConfiguration
    {
        private readonly Application _app;
        private readonly IViewResolver _viewResolver;
        private readonly IContainerConfiguration _containerConfiguration;
        private ByNameDiscoveryConvention _convention;

        public NyxApplicationConfiguration(Application app, IViewResolver viewResolver, IContainerConfiguration containerConfiguration)
        {
            _app = app;
            _viewResolver = viewResolver;
            _containerConfiguration = containerConfiguration;
        }

        private void AutoDiscoverViewModelsImpl(Assembly assembly)
        {
            if (_convention == null)
            {
                _convention = new ByNameDiscoveryConvention();
            }

            var mappings = _convention.DiscoverViewModelsAndViews(assembly);
            foreach (var m in mappings)
                _viewResolver.AddMapping(m.Key, m.Value);
        }


        public INyxApplicationConfiguration UsingDefaultConventions()
        {
            _convention = new ByNameDiscoveryConvention();
            return this;
        }

        public INyxApplicationConfiguration AutoDiscoverViewModels(Assembly assembly)
        {
            AutoDiscoverViewModelsImpl(assembly);
            return this;
        }

        public INyxApplicationConfiguration AutoDiscoverViewModels()
        {
            AutoDiscoverViewModelsImpl(_app.GetType().Assembly);
            return this;
        }

        public INyxApplicationConfiguration AutoDiscoverCommands(Assembly assembly)
        {
            AutoDiscoverCommandsImpl(assembly);
            return this;
        }

        private void AutoDiscoverCommandsImpl(Assembly assembly)
        {
            if (_convention == null)
            {
                _convention = new ByNameDiscoveryConvention();
            }

            var commands = _convention.DiscoverCommands(assembly);
            foreach (var commandType in commands)
                _containerConfiguration.Register(commandType);
        }

        public INyxApplicationConfiguration AutoDiscoverCommands()
        {
            AutoDiscoverCommandsImpl(_app.GetType().Assembly);
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