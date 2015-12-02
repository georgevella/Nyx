using System;

namespace Nyx.Composition
{
    /// <summary>
    /// Contract for the Pyxis container
    /// </summary>
    public interface IContainer : IDisposable
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
        ILifetime UsingLifetimeService();

        /// <summary>
        /// </summary>
        /// <param name="lifetime"></param>
        /// <typeparam name="TService"></typeparam>
        /// <returns></returns>
        TService Get<TService>(ILifetime lifetime);

        /// <summary>
        /// Creates a named instance of service type <typeparamref name="TService"/> 
        /// </summary>
        /// <param name="name">Name of service type to retrieve</param>
        /// <typeparam name="TService">Service type to retrieve</typeparam>
        /// <returns>An instance of service type <typeparamref name="TService"/></returns>
        TService Get<TService>(string name);

        /// <summary>
        /// Creates a named instance of service type <typeparamref name="TService"/> and 
        /// attaches the instance to the supplied Lifetime
        /// </summary>
        /// <param name="name">Name of service type to retrieve</param>
        /// <typeparam name="TService">Service type to retrieve</typeparam>
        /// <param name="lifetime">Lifetime instance to which the service intance will be attached to</param>		
        /// <returns></returns>
        TService Get<TService>(string name, ILifetime lifetime);


        object Get(Type serviceType);


        /// <summary>
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="lifetime"></param>
        /// <typeparam name="TService"></typeparam>
        /// <returns></returns>
        object Get(Type serviceType, ILifetime lifetime);

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
        /// attaches the instance to the supplied Lifetime
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="name">Name of service type to retrieve</param>
        /// <typeparam name="TService">Service type to retrieve</typeparam>
        /// <param name="lifetime">Lifetime instance to which the service intance will be attached to</param>		
        /// <returns></returns>
        object Get(Type serviceType, string name, ILifetime lifetime);
    }

    /// <summary>
    /// Service Lifetime contract
    /// </summary>
    public interface ILifetime : IDisposable
    {

    }
}