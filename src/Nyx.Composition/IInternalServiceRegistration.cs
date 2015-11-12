using System;

namespace Nyx.Composition
{
    /// <summary>
    /// </summary>
    internal interface IInternalServiceRegistration
    {
        /// <summary>
        /// .NET Type that this registration will handle
        /// </summary>
        Type ContractType { get; }

        /// <summary>
        /// Name of registration
        /// </summary>
        string Name { get; }
        /// <summary>
        /// .NET Type that will be constructed when <see cref="ContractType"/> is requested
        /// </summary>
        Type TargetType { get; }

        bool SupportsInstanceBuilder { get; }

        /// <summary>
        /// Returns an instance builder instance that can be used to populate properties in a constructed contract
        /// </summary>
        /// <returns>An initialized instance of <see cref="IInstanceBuilder"/></returns>
        IInstanceBuilder GetInstanceBuilder();

        /// <summary>
        /// Returns a factory instance that is used to create services of the
        /// type <see cref="IInternalServiceRegistration.TargetType" />
        /// </summary>
        /// <returns>
        /// An instance of either <c>SimpleServiceFactory</c> or <c>ConstructorInjectionServiceFactory</c>
        /// </returns>
        IServiceFactory GetObjectFactory();
    }

    public interface IServiceRegistration
    {
        IServiceRegistration Using(object serviceInstance);

        IServiceRegistration UsingConcreteType(Type type);

        IServiceRegistration Named(string name);
    }

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
}