using Nyx.Composition.Impl;

namespace Nyx.Composition
{
    internal interface IServiceFactory
    {
        object Create(ServiceInstantiationGraph instantiationGraph, AbstractLifeTime lifetime);
    }
}