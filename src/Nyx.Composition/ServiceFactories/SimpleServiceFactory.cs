using System;
using System.Linq.Expressions;
using System.Reflection;
using Nyx.Composition.Impl;

namespace Nyx.Composition.ServiceFactories
{
    internal class SimpleServiceFactory : IServiceFactory
    {
        private readonly Func<object> _constructorFunc;

        private readonly Type _targetType;

        public SimpleServiceFactory(Type targetType, ConstructorInfo constructor)
        {
            _targetType = targetType;

            var newExpression = Expression.New(constructor);

            var ctorExpression = Expression.Lambda<Func<object>>(newExpression);
            _constructorFunc = ctorExpression.Compile();

        }

        public object Create(ServiceInstantiationGraph instantiationGraph, AbstractLifeTime lifetime)
        {
            return _constructorFunc();
        }
    }
}