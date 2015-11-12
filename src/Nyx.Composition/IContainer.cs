using System;

namespace Nyx.Composition
{
    /// <summary>
    /// Contract for the Pyxis container
    /// </summary>
    public interface IContainer
    {
        /// <summary>
        /// Creates an instance of service type <typeparamref name="TService"/>
        /// </summary>		
        /// <typeparam name="TService">Service type to retrieve</typeparam>
        /// <returns>An instance of service type <typeparamref name="TService"/></returns>
        TService Get<TService>();

        /// <summary>
        /// </summary>
        /// <returns></returns>
        ILifecycle BeginLifecycle();

        /// <summary>
        /// </summary>
        /// <param name="lifecycle"></param>
        /// <typeparam name="TService"></typeparam>
        /// <returns></returns>
        TService Get<TService>(ILifecycle lifecycle);

        /// <summary>
        /// Creates a named instance of service type <typeparamref name="TService"/> 
        /// </summary>
        /// <param name="name">Name of service type to retrieve</param>
        /// <typeparam name="TService">Service type to retrieve</typeparam>
        /// <returns>An instance of service type <typeparamref name="TService"/></returns>
        TService Get<TService>(string name);

        /// <summary>
        /// Creates a named instance of service type <typeparamref name="TService"/> and 
        /// attaches the instance to the supplied lifecycle
        /// </summary>
        /// <param name="name">Name of service type to retrieve</param>
        /// <typeparam name="TService">Service type to retrieve</typeparam>
        /// <param name="lifecycle">Lifecycle instance to which the service intance will be attached to</param>		
        /// <returns></returns>
        TService Get<TService>(string name, ILifecycle lifecycle);


        object Get(Type serviceType);


        /// <summary>
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="lifecycle"></param>
        /// <typeparam name="TService"></typeparam>
        /// <returns></returns>
        object Get(Type serviceType, ILifecycle lifecycle);

        /// <summary>
        /// Creates a named instance of service type <typeparamref name="TService"/> 
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="name">Name of service type to retrieve</param>
        /// <typeparam name="TService">Service type to retrieve</typeparam>
        /// <returns>An instance of service type <typeparamref name="TService"/></returns>
        object Get(Type serviceType, string name);

        /// <summary>
        /// Creates a named instance of service type <typeparamref name="TService"/> and 
        /// attaches the instance to the supplied lifecycle
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="name">Name of service type to retrieve</param>
        /// <typeparam name="TService">Service type to retrieve</typeparam>
        /// <param name="lifecycle">Lifecycle instance to which the service intance will be attached to</param>		
        /// <returns></returns>
        object Get(Type serviceType, string name, ILifecycle lifecycle);
    }

    /// <summary>
    /// Service lifecycle contract
    /// </summary>
    public interface ILifecycle : IDisposable
    {

    }
}