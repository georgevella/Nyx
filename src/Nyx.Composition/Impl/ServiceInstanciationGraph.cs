using System;
using System.Collections.Generic;

namespace Nyx.Composition.Impl
{
    /// <summary>
    /// Keeps track of the objects created by the container during a single operation
    /// </summary>
    internal class ServiceInstantiationGraph
    {
        private readonly ContainerImpl _container;
        private readonly Dictionary<ServiceKey, object> _instances = new Dictionary<ServiceKey, object>();

        /// <summary>
        /// Creates an instance of <c>ServiceInstantiationGraph</c>
        /// </summary>
        /// <param name="container"></param>
        public ServiceInstantiationGraph(ContainerImpl container)
        {
            _container = container;
        }

        /// <summary>
        /// Fetches an instance of <paramref name="contractType"/> in the current graph, or asks the container for a new instance 
        /// </summary>
        /// <param name="contractType">Type of service required</param>
        /// <returns>Instance of <paramref name="contractType"/></returns>
        public object Get(Type contractType)
        {
            var key = new ServiceKey(contractType);
            if (_instances.ContainsKey(key))
                return _instances[key];

            var instance = _container.Get(this, contractType);
            _instances[key] = instance;

            return instance;

        }
    }
}