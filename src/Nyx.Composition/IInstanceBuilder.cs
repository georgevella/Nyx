using Nyx.Composition.Impl;

namespace Nyx.Composition
{
    /// <summary>
    /// </summary>
    internal interface IInstanceBuilder
    {
        void Build(ServiceInstantiationGraph instantiationGraph, object instance);
    }
}