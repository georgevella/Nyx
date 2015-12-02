using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Nyx.Composition.Impl;

namespace Nyx.Composition.ServiceFactories
{
    /// <summary>
    /// Uses constructor injection to create an instance of a service
    /// </summary>
    internal class ConstructorInjectionServiceFactory<TService> : IServiceFactory
    {
        private readonly Action<TService> _initializerMethod;
        private readonly List<Type> _parameterTypes = new List<Type>();

        private readonly Func<object[], object> _constructor;

        /// <summary>
        /// Creates an instance of ConstructorInjectionServiceFactory
        /// </summary>
        /// <param name="constructor">Constructor to use to create service</param>
        /// <param name="configuration">Container configuration</param>
        /// <param name="initializerMethod"></param>
        /// <exception cref="InvalidOperationException">If constructor defines an
        /// output parameter, or a non optional parameter is not supported by the
        /// current container configuration</exception>
        public ConstructorInjectionServiceFactory(ConstructorInfo constructor, FluentContainerConfigurator configuration, Action<TService> initializerMethod = null)
        {
            _initializerMethod = initializerMethod;

            var argsParam = Expression.Parameter(typeof(object[]), "args");
            var ctorArgs = new List<Expression>();

            var parameters = constructor.GetParameters();

            for (int i = 0; i < parameters.Length; i++)
            {
                var p = parameters[i];
                if (p.IsOut)
                {
                    throw new InvalidOperationException("Out parameters in constructor are not supported");
                }

                var reg = configuration.Registrations.FirstOrDefault(r => r.ContractType == p.ParameterType);
                if (reg == null)
                {
                    if (!p.IsOptional)
                        throw new InvalidOperationException("Cannot find registration for service [" + p.ParameterType +
                                                                                                "]");

                    if (!p.HasDefaultValue)
                        throw new InvalidOperationException("Default value for service [" + p.ParameterType + "] not present");

                    var defaultValueExpression = Expression.Constant(p.DefaultValue);
                    var constructorParameterExpression = Expression.MakeUnary(ExpressionType.Convert,
                            defaultValueExpression,
                            p.ParameterType);
                    ctorArgs.Add(constructorParameterExpression);
                }
                else
                {
                    _parameterTypes.Add(p.ParameterType);

                    // build constructor arameters
                    var arrayIndexExpression = Expression.MakeBinary(ExpressionType.ArrayIndex, argsParam,
                            Expression.Constant(i));
                    var constructorParameterExpression = Expression.MakeUnary(ExpressionType.Convert,
                            arrayIndexExpression,
                            p.ParameterType);
                    ctorArgs.Add(constructorParameterExpression);
                }
            }

            var newExpression = Expression.New(constructor, ctorArgs);
            var ctorExpression = Expression.Lambda<Func<object[], object>>(newExpression, argsParam);
            _constructor = ctorExpression.Compile();
        }

        public object Create(ServiceInstantiationGraph instantiationGraph, AbstractLifeTime lifetime)
        {
            var args = new object[_parameterTypes.Count];
            var argsIndex = 0;
            foreach (var paramType in _parameterTypes)
            {
                args[argsIndex++] = instantiationGraph.Get(paramType, lifetime);
            }

            var obj = _constructor(args);

            if (_initializerMethod != null)
                _initializerMethod((TService)obj);


            return obj;
        }
    }
}