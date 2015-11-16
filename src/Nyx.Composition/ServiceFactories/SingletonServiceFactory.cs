using System;
using Nyx.Composition.Impl;

namespace Nyx.Composition.ServiceFactories
{
    internal class SingletonServiceFactory : IServiceFactory
    {
        private readonly IServiceFactory _factory;
        private object _instance = null;

        public SingletonServiceFactory(IServiceFactory factory)
        {
            _factory = factory;
        }

        public object Create(ServiceInstantiationGraph instantiationGraph, AbstractLifeTime lifetime)
        {
            if (_instance == null)
                _instance = _factory.Create(instantiationGraph, lifetime);

            return _instance;
        }
    }
}