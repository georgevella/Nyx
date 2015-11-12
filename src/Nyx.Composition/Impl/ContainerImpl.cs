using System;
using System.Collections.Generic;

namespace Nyx.Composition.Impl
{
    /// <summary>
    /// </summary>
    internal class ContainerImpl : IContainer, IServiceProvider
    {
        private readonly FluentContainerConfigurator _configuration;
        private readonly Dictionary<ServiceKey, IServiceFactory> _factories = new Dictionary<ServiceKey, IServiceFactory>();
        private readonly Dictionary<ServiceKey, IInstanceBuilder> _instanceBuilders = new Dictionary<ServiceKey, IInstanceBuilder>();

        /// <summary>
        /// Creates a new instance of <see cref="FluentContainerConfigurator"/>
        /// </summary>
        /// <param name="configuration"></param>
        public ContainerImpl(FluentContainerConfigurator configuration)
        {
            _configuration = configuration;
        }


        public TService Get<TService>()
        {
            var type = typeof(TService);
            var context = new ServiceInstantiationGraph(this);
            return (TService)Get(context, type);
        }

        public ILifecycle BeginLifecycle() => new Lifecycle();

        public TService Get<TService>(ILifecycle lifecycle)
        {
            var lifeCycleInstance = (Lifecycle)lifecycle;

            var obj = Get<TService>();
            lifeCycleInstance.Register(obj);

            return obj;
        }

        public TService Get<TService>(string name)
        {
            var type = typeof(TService);
            var context = new ServiceInstantiationGraph(this);
            return (TService)Get(context, type, name);
        }

        public TService Get<TService>(string name, ILifecycle lifecycle)
        {
            throw new NotImplementedException();
        }

        public object Get(Type serviceType)
        {
            var context = new ServiceInstantiationGraph(this);
            return Get(context, serviceType);
        }

        public object Get(Type serviceType, ILifecycle lifecycle)
        {
            var lifeCycleInstance = (Lifecycle)lifecycle;

            var obj = Get(serviceType);
            lifeCycleInstance.Register(obj);

            return obj;
        }

        public object Get(Type serviceType, string name)
        {
            var context = new ServiceInstantiationGraph(this);
            return Get(context, serviceType, name);
        }

        public object Get(Type serviceType, string name, ILifecycle lifecycle)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Compiles the container configuration and generates required data structures to be able to build the registered types
        /// </summary>
        public void Build()
        {
            foreach (var reg in _configuration.Registrations)
            {
                var key = new ServiceKey(reg);
                _factories.Add(key, reg.GetObjectFactory());
                if (reg.SupportsInstanceBuilder)
                    _instanceBuilders.Add(key, reg.GetInstanceBuilder());
            }
        }

        /// <summary>
        /// Creates an instance of the service type specified in <paramref name="contractType"/>
        /// </summary>
        /// <param name="instantiationGraph">Instanciation instanciationGraph</param>
        /// <param name="contractType">Service to instanciate</param>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="contractType"/> and/or <paramref name="instantiationGraph"/> is <see langword="null" />.</exception>
        internal object Get(ServiceInstantiationGraph instantiationGraph, Type contractType, string name = null)
        {
            if (contractType == null)
            {
                throw new ArgumentNullException(nameof(contractType));
            }
            if (instantiationGraph == null)
            {
                throw new ArgumentNullException(nameof(instantiationGraph));
            }
            var key = new ServiceKey(contractType, name);
            var factory = _factories[key];

            var instance = factory.Create(instantiationGraph);

            // if registration does not support instance building (it's a singleton / set up with Using(instance) method
            // an instancebuilder won't be present
            if (_instanceBuilders.ContainsKey(key))
            {
                var builder = _instanceBuilders[key];
                builder.Build(instantiationGraph, instance);
            }

            return instance;
        }

        object IServiceProvider.GetService(Type type)
        {
            var context = new ServiceInstantiationGraph(this);
            return Get(context, type);
        }
    }

    internal class Lifecycle : ILifecycle
    {
        private readonly List<IDisposable> _disposables = new List<IDisposable>();
        public void Dispose()
        {
            foreach (var item in _disposables)
            {
                item.Dispose();
            }

            _disposables.Clear();
        }

        public void Register(object instance)
        {
            var disposable = instance as IDisposable;
            if (disposable != null)
            {
                _disposables.Add(disposable);
            }
        }
    }
}