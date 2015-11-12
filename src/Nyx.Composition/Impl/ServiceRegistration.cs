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
        private readonly Type _contractType;
        public bool IsTransient { get; private set; }

        public Type TargetType => _targetType;

        public Type ContractType => _contractType;

        public string Name => _name;

        public bool SupportsInstanceBuilder => _targetType != null;

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
            return _targetType == other.TargetType && _contractType == other.ContractType && string.Equals(Name, other.Name);
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
                var hashCode = _targetType?.GetHashCode() ?? 0;
                hashCode = (hashCode * 397) ^ (_contractType?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (Name?.GetHashCode() ?? 0);
                return hashCode;
            }
        }

        public ServiceRegistration(FluentContainerConfigurator parentConfiguration)
        {
            _parentConfiguration = parentConfiguration;
            _contractType = typeof(TService);

            if (!ContractType.GetTypeInfo().IsInterface)
            {
                _targetType = _contractType;
            }
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

            if (serviceInstance is Type)
                throw new InvalidOperationException("Use UsingConcreteType() instead when providing Type information");

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
            IsTransient = true;

            return this;
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

            var typeInfo = _targetType.GetTypeInfo();
            var constructors = typeInfo.DeclaredConstructors.ToList();

            if (constructors.Count == 1)
            {
                var constructor = constructors.First();
                var parameters = constructor.GetParameters();
                if (parameters.Length == 0)
                {
                    return new SimpleServiceFactory(_contractType, constructor);
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