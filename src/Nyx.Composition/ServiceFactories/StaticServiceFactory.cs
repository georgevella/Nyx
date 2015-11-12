using Nyx.Composition.Impl;

namespace Nyx.Composition.ServiceFactories
{
    class StaticServiceFactory : IServiceFactory
    {
        private readonly object _staticInstance;

        public StaticServiceFactory(object staticInstance)
        {
            _staticInstance = staticInstance;
        }

        public object Create(ServiceInstantiationGraph instantiationGraph)
        {
            return _staticInstance;
        }
    }
}