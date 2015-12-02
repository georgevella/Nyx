using System;
using System.Collections.Generic;

namespace Nyx.Composition.Impl
{
    /// <summary>
    /// </summary>
    internal class ContainerImpl : IContainer, IServiceProvider
    {
        public FluentContainerConfigurator Configuration { get; }
        private readonly Dictionary<ServiceKey, IInternalServiceRegistration> _registrations = new Dictionary<ServiceKey, IInternalServiceRegistration>();
        private readonly Dictionary<ServiceKey, IServiceFactory> _factories = new Dictionary<ServiceKey, IServiceFactory>();
        private readonly Dictionary<ServiceKey, IInstanceBuilder> _instanceBuilders = new Dictionary<ServiceKey, IInstanceBuilder>();
        private readonly AbstractLifeTime _defaultLifeTime = new DisposeAtEndOfLife();

        /// <summary>
        /// Creates a new instance of <see cref="FluentContainerConfigurator"/>
        /// </summary>
        /// <param name="configuration"></param>
        public ContainerImpl(FluentContainerConfigurator configuration)
        {
            Configuration = configuration;
        }


        public TService Get<TService>()
        {
            return (TService)ResolveAndBuildService(typeof(TService), _defaultLifeTime);
        }

        public ILifetime UsingLifetimeService() => new DisposeAtEndOfLife();

        public TService Get<TService>(ILifetime lifetime)
        {
            return (TService)ResolveAndBuildService(typeof(TService), (AbstractLifeTime)lifetime);
        }

        public TService Get<TService>(string name)
        {
            return (TService)ResolveAndBuildService(typeof(TService), _defaultLifeTime, name: name);
        }

        public TService Get<TService>(string name, ILifetime lifetime)
        {
            return (TService)ResolveAndBuildService(typeof(TService), (AbstractLifeTime)lifetime, name: name);
        }

        public object Get(Type serviceType)
        {
            return ResolveAndBuildService(serviceType, _defaultLifeTime);
        }

        public object Get(Type serviceType, ILifetime lifetime)
        {
            return ResolveAndBuildService(serviceType, (AbstractLifeTime)lifetime);
        }

        public object Get(Type serviceType, string name)
        {
            return ResolveAndBuildService(serviceType, _defaultLifeTime, name: name);
        }

        public object Get(Type serviceType, string name, ILifetime lifetime)
        {
            return ResolveAndBuildService(serviceType, (AbstractLifeTime)lifetime, name: name);
        }

        /// <summary>
        /// Compiles the container configuration and generates required data structures to be able to build the registered types
        /// </summary>
        public void Build()
        {
            foreach (var reg in Configuration.Registrations)
            {
                var key = new ServiceKey(reg);
                _registrations.Add(key, reg);
                _factories.Add(key, reg.GetObjectFactory());
                if (reg.SupportsInstanceBuilder)
                    _instanceBuilders.Add(key, reg.GetInstanceBuilder());
            }
        }

        /// <summary>
        /// Creates an instance of the service type specified in <paramref name="contractType"/>
        /// </summary>
        /// <param name="contractType">Service to instanciate</param>
        /// <param name="lifetime"></param>
        /// <param name="instantiationGraph">Instanciation instanciationGraph</param>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="contractType"/> and/or <paramref name="instantiationGraph"/> is <see langword="null" />.</exception>
        internal object ResolveAndBuildService(Type contractType, AbstractLifeTime lifetime, ServiceInstantiationGraph instantiationGraph = null, string name = null)
        {
            if (contractType == null)
            {
                throw new ArgumentNullException(nameof(contractType));
            }
            if (instantiationGraph == null)
            {
                instantiationGraph = new ServiceInstantiationGraph(this);
            }
            var key = new ServiceKey(contractType, name);
            var reg = _registrations[key];
            var factory = _factories[key];

            if (!reg.IsTransient)
            {
                // registration is not transient, check into graph for an instance and return that
                if (instantiationGraph.Instances.ContainsKey(key))
                    return instantiationGraph.Instances[key];
            }

            var instance = factory.Create(instantiationGraph, lifetime);

            // if registration does not support instance building (it's a singleton / set up with Using(instance) method
            // an instancebuilder won't be present
            if (_instanceBuilders.ContainsKey(key))
            {
                var builder = _instanceBuilders[key];
                builder.Build(instantiationGraph, instance, lifetime);
            }

            // we never add the container to the lifetime instance
            if (!(instance is IContainer))
                lifetime.Register(instance);

            return instance;
        }

        object IServiceProvider.GetService(Type type)
        {
            var context = new ServiceInstantiationGraph(this);
            return ResolveAndBuildService(type, _defaultLifeTime, context);
        }

        public void Dispose()
        {
            _defaultLifeTime.Dispose();
        }
    }
}