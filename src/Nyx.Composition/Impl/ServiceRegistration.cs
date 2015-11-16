using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Nyx.Composition.ServiceFactories;

namespace Nyx.Composition.Impl
{
    /// <summary>
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    internal class ServiceRegistration<TService> : IServiceRegistration<TService>, IInternalServiceRegistration, IServiceRegistration, IEquatable<ServiceRegistration<TService>>
    {
        private readonly FluentContainerConfigurator _parentConfiguration;
        private string _name;
        private TService _staticInstance;
        public bool IsTransient { get; private set; }

        public bool IsSingleton { get; private set; }

        public Type InstanceType { get; private set; }

        public Type ContractType { get; }

        public string Name => _name;

        public bool SupportsInstanceBuilder => InstanceType != null;


        private void UsingConcreteTypeImpl(Type type)
        {
            _staticInstance = default(TService);
            InstanceType = type;
            IsSingleton = false;
        }

        private void UsingStaticInstanceImpl(TService serviceInstance)
        {
            _staticInstance = serviceInstance;
            InstanceType = null;
            IsSingleton = true;
        }


        public bool Equals(ServiceRegistration<TService> other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return InstanceType == other.InstanceType && ContractType == other.ContractType && string.Equals(Name, other.Name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            var other = obj as ServiceRegistration<TService>;
            return other != null && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = InstanceType?.GetHashCode() ?? 0;
                hashCode = (hashCode * 397) ^ (ContractType?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (Name?.GetHashCode() ?? 0);
                return hashCode;
            }
        }

        public ServiceRegistration(FluentContainerConfigurator parentConfiguration)
        {
            _parentConfiguration = parentConfiguration;
            ContractType = typeof(TService);

            if (!ContractType.GetTypeInfo().IsInterface)
            {
                InstanceType = ContractType;
            }
        }

        public IServiceRegistration<TService> UsingConcreteType(Type type)
        {
            UsingConcreteTypeImpl(type);
            return this;
        }

        public IServiceRegistration<TService> UsingConcreteType<TConcreteType>() where TConcreteType : class, TService
        {
            return UsingConcreteType(typeof(TConcreteType));
        }

        IServiceRegistration IServiceRegistration.UsingConcreteType(Type type)
        {
            return (IServiceRegistration)UsingConcreteType(type);
        }

        public IServiceRegistration Using(object serviceInstance)
        {
            if (serviceInstance == null)
                throw new ArgumentNullException(nameof(serviceInstance));

            if (serviceInstance is Type)
                throw new InvalidOperationException("Use UsingConcreteType() instead when providing Type information");

            if (!(serviceInstance is TService))
                throw new InvalidOperationException("Invalid instance type");

            UsingStaticInstanceImpl((TService)serviceInstance);
            return this;
        }

        public IServiceRegistration<TService> Using(TService serviceInstance)
        {
            if (serviceInstance == null)
                throw new ArgumentNullException(nameof(serviceInstance));

            UsingStaticInstanceImpl(serviceInstance);
            return this;
        }


        IServiceRegistration IServiceRegistration.Named(string name)
        {
            return (IServiceRegistration)Named(name);
        }

        public IServiceRegistration<TService> Named(string name)
        {
            _name = name;
            return this;
        }

        public IServiceRegistration<TService> AsSingleton()
        {
            IsSingleton = true;
            IsTransient = false;
            return this;
        }

        public IServiceRegistration<TService> AsTransient()
        {
            IsTransient = true;
            IsSingleton = false;
            return this;
        }

        public IServiceRegistration<TService> InitializeWith(Action<TService> initializerMethod)
        {
            throw new NotImplementedException();
        }

        public IInstanceBuilder GetInstanceBuilder()
        {
            var instanceBuilderType = typeof(InstanceBuilder<>).MakeGenericType(InstanceType);
            var instanceBuilder = (IInstanceBuilder)Activator.CreateInstance(instanceBuilderType);

            return instanceBuilder;
        }

        public IServiceFactory GetObjectFactory()
        {
            if (!IsSingleton)
            {
                return InternalServiceFactoryFactory();
            }

            if (_staticInstance != null)
            {
                return new StaticServiceFactory(_staticInstance);
            }

            var factory = InternalServiceFactoryFactory();
            return new SingletonServiceFactory(factory);
        }

        private IServiceFactory InternalServiceFactoryFactory()
        {
            var typeInfo = InstanceType.GetTypeInfo();
            var constructors = typeInfo.DeclaredConstructors.ToList();

            if (constructors.Count == 1)
            {
                var constructor = constructors.First();
                var parameters = constructor.GetParameters();
                if (parameters.Length == 0)
                {
                    return new SimpleServiceFactory(ContractType, constructor);
                }
                return new ConstructorInjectionServiceFactory(constructor, _parentConfiguration);
            }

            // find the most suitable constructor to use (the one which we can support the most with the current registrations)
            var ctor = DetermineBestConstructor(constructors);
            if (ctor == null)
            {
                throw new InvalidOperationException("Unable to find suitable constructor for [" + InstanceType + "]");
            }

            return new ConstructorInjectionServiceFactory(ctor, _parentConfiguration);
        }

        private ConstructorInfo DetermineBestConstructor(List<ConstructorInfo> constructors)
        {
            var items = constructors.Select(c => new Tuple<ConstructorInfo, ParameterInfo[]>(c, c.GetParameters()))
                .ToList();

            items.Sort((x, y) => y.Item2.Length - x.Item2.Length);

            foreach (var ctorMetadata in items)
            {
                bool allParametersResolvable = true;

                foreach (var parameter in ctorMetadata.Item2)
                {
                    if (_parentConfiguration.Registrations.All(v => v.ContractType != parameter.ParameterType) &&
                        !parameter.IsOptional)
                    {
                        allParametersResolvable = false;
                        break;
                    }
                }

                if (!allParametersResolvable)
                {
                    continue;
                }

                return ctorMetadata.Item1;
            }

            return null;
        }
    }
}