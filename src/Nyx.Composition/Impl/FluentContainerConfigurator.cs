using System;
using System.Collections.Generic;
using System.Reflection;

namespace Nyx.Composition.Impl
{
    /// <summary>
    /// </summary>
    internal class FluentContainerConfigurator : IContainerConfiguration
    {
        private readonly List<IInternalServiceRegistration> _registrations = new List<IInternalServiceRegistration>();

        /// <summary>
        /// </summary>
        /// <typeparam name="TService">Service type to register with container</typeparam>
        /// <returns>Fluent registration helper class to build the registration</returns>
        /// <exception cref="InvalidOperationException"><typeparamref name="TService"/> is not an interface.</exception>
        public IServiceRegistration<TService> Register<TService>()
        {
            var type = typeof(TService);
            if (!type.GetTypeInfo().IsInterface)
                throw new InvalidOperationException();

            var pyxisRegistration = new ServiceRegistration<TService>(this);

            _registrations.Add(pyxisRegistration);

            return pyxisRegistration;
        }

        public IServiceRegistration Register(Type type)
        {
            var registrationType = typeof(ServiceRegistration<>).MakeGenericType(type);
            var reg = (IServiceRegistration)Activator.CreateInstance(registrationType, this);

            _registrations.Add((IInternalServiceRegistration)reg);

            return reg;
        }

        /// <summary>
        /// </summary>
        public IReadOnlyList<IInternalServiceRegistration> Registrations
        {
            get { return _registrations; }
        }
    }
}