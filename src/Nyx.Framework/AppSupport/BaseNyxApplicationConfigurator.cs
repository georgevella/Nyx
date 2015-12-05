using System;
using System.Reflection;
using Nyx.Composition;
using Nyx.Presentation;
using Nyx.Presentation.Conventions;

namespace Nyx.AppSupport
{
    public abstract class BaseNyxApplicationConfigurator : INyxApplicationConfiguration
    {
        private readonly IViewResolver _viewResolver;
        private readonly IContainerConfiguration _containerConfiguration;
        private IDiscoveryConvention _convention;

        protected BaseNyxApplicationConfigurator(IViewResolver viewResolver, IContainerConfiguration containerConfiguration)
        {
            _viewResolver = viewResolver;
            _containerConfiguration = containerConfiguration;
        }

        protected void AutoDiscoverViewModelsImpl(Assembly assembly)
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

        public abstract INyxApplicationConfiguration AutoDiscoverViewModels();

        public INyxApplicationConfiguration AutoDiscoverCommands(Assembly assembly)
        {
            AutoDiscoverCommandsImpl(assembly);
            return this;
        }

        public abstract INyxApplicationConfiguration AutoDiscoverCommands();

        protected void AutoDiscoverCommandsImpl(Assembly assembly)
        {
            if (_convention == null)
            {
                _convention = new ByNameDiscoveryConvention();
            }

            var commands = _convention.DiscoverCommands(assembly);
            foreach (var commandType in commands)
                _containerConfiguration.Register(commandType);
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