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
        bool IsTransient { get; }

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
}