using System;
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
        private Type _targetType;
        private string _name;
        private TService _staticInstance;

        public Type TargetType => _targetType;

        public Type ContractType { get; }

        public string Name => _name;

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
            return TargetType == other.TargetType && ContractType == other.ContractType && string.Equals(Name, other.Name);
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
                var hashCode = (_targetType != null ? TargetType.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ContractType?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (Name?.GetHashCode() ?? 0);
                return hashCode;
            }
        }

        public ServiceRegistration(FluentContainerConfigurator parentConfiguration)
        {
            _parentConfiguration = parentConfiguration;
            ContractType = typeof(TService);
        }

        public IServiceRegistration<TService> UsingConcreteType(Type type)
        {
            _targetType = type;
            _staticInstance = default(TService);
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

            if (!(serviceInstance is TService))
                throw new InvalidOperationException("Invalid instance type");

            Using((TService)serviceInstance);
            return this;
        }

        public IServiceRegistration<TService> Using(TService serviceInstance)
        {
            if (serviceInstance == null)
                throw new ArgumentNullException(nameof(serviceInstance));

            _staticInstance = (TService)serviceInstance;
            _targetType = null;
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
            throw new NotImplementedException();
        }

        public IServiceRegistration<TService> AsTransient()
        {
            throw new NotImplementedException();
        }

        public IServiceRegistration<TService> InitializeWith(Action<TService> initializerMethod)
        {
            throw new NotImplementedException();
        }

        public IInstanceBuilder GetInstanceBuilder()
        {
            var instanceBuilderType = typeof(InstanceBuilder<>).MakeGenericType(_targetType);
            var instanceBuilder = (IInstanceBuilder)Activator.CreateInstance(instanceBuilderType);

            return instanceBuilder;
        }

        public IServiceFactory GetObjectFactory()
        {
            if (_staticInstance != null)
                return new StaticServiceFactory(_staticInstance);

            var targetType = TargetType;
            var typeInfo = targetType.GetTypeInfo();
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
                    continue;

                return new ConstructorInjectionServiceFactory(ctorMetadata.Item1, _parentConfiguration);
            }

            throw new InvalidOperationException();
        }
    }
}