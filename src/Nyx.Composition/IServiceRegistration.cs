using System;

namespace Nyx.Composition
{
    /// <summary>
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    public interface IServiceRegistration<TService>
    {
        /// <summary>
        /// </summary>
        /// <typeparam name="TConcreteType"></typeparam>
        /// <returns>Self instance of <see cref="IInternalServiceRegistration"/></returns>
        IServiceRegistration<TService> UsingConcreteType<TConcreteType>() where TConcreteType : class, TService;

        IServiceRegistration<TService> UsingConcreteType(Type type);

        /// <summary>        
        /// </summary>
        /// <param name="name"></param>
        /// <returns>Self instance of <see cref="IInternalServiceRegistration"/></returns>
        IServiceRegistration<TService> Named(string name);

        /// <summary>
        /// There will be only one instance of the registered service type during the lifetime of that container instance.
        /// </summary>
        /// <returns>Self instance of <see cref="IInternalServiceRegistration"/></returns>
        IServiceRegistration<TService> AsSingleton();

        /// <summary>
        /// A new instance of the service type will be created for each request
        /// </summary>
        /// <returns>Self instance of <see cref="IInternalServiceRegistration"/></returns>
        IServiceRegistration<TService> AsTransient();

        IServiceRegistration<TService> InitializeWith(Action<TService> initializerMethod);
        IServiceRegistration<TService> Using(TService serviceInstance);
    }

    public interface IServiceRegistration
    {
        IServiceRegistration Using(object serviceInstance);

        IServiceRegistration UsingConcreteType(Type type);

        IServiceRegistration Named(string name);
    }
}