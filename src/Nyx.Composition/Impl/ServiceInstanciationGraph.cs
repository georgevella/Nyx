using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Nyx.Composition.Impl
{
    /// <summary>
    /// Keeps track of the objects created by the container during a single operation
    /// </summary>
    internal class ServiceInstantiationGraph
    {
        private readonly ContainerImpl _container;
        public Dictionary<ServiceKey, object> Instances { get; } = new Dictionary<ServiceKey, object>();

        /// <summary>
        /// Creates an instance of <c>ServiceInstantiationGraph</c>
        /// </summary>
        /// <param name="container"></param>
        public ServiceInstantiationGraph(ContainerImpl container)
        {
            _container = container;
        }

        public bool IsTypeSupported(Type contractType)
        {
            return _container.Configuration.Registrations.Any(x => x.ContractType == contractType);
        }

        /// <summary>
        /// Fetches an instance of <paramref name="contractType"/> in the current graph, or asks the container for a new instance 
        /// </summary>
        /// <param name="contractType">Type of service required</param>
        /// <param name="lifetime"></param>
        /// <returns>Instance of <paramref name="contractType"/></returns>
        public object Get(Type contractType, AbstractLifeTime lifetime)
        {
            var key = new ServiceKey(contractType);
            var instance = _container.ResolveAndBuildService(contractType, lifetime, this);
            Instances[key] = instance;

            return instance;

        }
    }
}