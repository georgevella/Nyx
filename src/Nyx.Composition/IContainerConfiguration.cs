using System;

namespace Nyx.Composition
{
    /// <summary>
    /// Pyxis container configuration system
    /// </summary>
    public interface IContainerConfiguration
    {
        /// <summary>
        /// Registers a service of type <typeparamref name="TService"/> with the container 
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <returns></returns>
        IServiceRegistration<TService> Register<TService>();
        IServiceRegistration Register(Type type);
    }
}