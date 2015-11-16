using System;
using System.Linq.Expressions;
using System.Reflection;
using Nyx.Composition.Impl;

namespace Nyx.Composition.ServiceFactories
{
    internal class SimpleServiceFactory<TService> : IServiceFactory
    {
        private readonly Func<TService> _constructorFunc;

        private readonly Type _targetType;
        private readonly Action<TService> _initializerMethod;

        public SimpleServiceFactory(Type targetType, ConstructorInfo constructor, Action<TService> initializerMethod = null)
        {
            _targetType = targetType;
            _initializerMethod = initializerMethod;

            var newExpression = Expression.New(constructor);

            var ctorExpression = Expression.Lambda<Func<TService>>(newExpression);
            _constructorFunc = ctorExpression.Compile();

        }

        public object Create(ServiceInstantiationGraph instantiationGraph, AbstractLifeTime lifetime)
        {
            var obj = _constructorFunc();

            if (_initializerMethod != null)
                _initializerMethod(obj);

            return obj;
        }
    }
}