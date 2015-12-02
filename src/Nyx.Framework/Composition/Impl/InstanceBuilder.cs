using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Nyx.Composition.Impl
{
    /// <summary>
    /// Populates properties with service instances 
    /// </summary>
    /// <typeparam name="TConcreteType"></typeparam>
    internal class InstanceBuilder<TConcreteType> : IInstanceBuilder where TConcreteType : class
    {
        private readonly List<Tuple<Type, Action<TConcreteType, object>>> _propSetters = new List<Tuple<Type, Action<TConcreteType, object>>>();

        /// <summary>
        /// Creates a new instance of <c>InstanceBuilder</c>
        /// </summary>
        public InstanceBuilder()
        {
            var concreteType = typeof(TConcreteType).GetTypeInfo();
            foreach (var property in concreteType.DeclaredProperties)
            {
                // filter out readonly properties
                if (!property.CanWrite)
                    continue;

                // filter out properties that aren't class or interface return types
                var propertyTypeInfo = property.PropertyType.GetTypeInfo();
                if (!propertyTypeInfo.IsClass && !propertyTypeInfo.IsInterface)
                    continue;

                // this is probably redundant but it's left here for posterity sake
                var setterMethod = property.SetMethod;
                if (setterMethod == null)
                    continue;

                // we need to build the following lambda: (target) => target.{Prop} = value
                var target = Expression.Parameter(typeof(TConcreteType), "target");
                var value = Expression.Parameter(typeof(object), "value");
                var cast = Expression.Convert(value, property.PropertyType);


                var call = Expression.Call(target, setterMethod, cast);
                var lambda = Expression.Lambda<Action<TConcreteType, object>>(call, target, value);

                _propSetters.Add(new Tuple<Type, Action<TConcreteType, object>>(property.PropertyType, lambda.Compile()));
            }
        }


        /// <summary>
        /// Populates all properties with the respective services
        /// </summary>
        /// <param name="instantiationGraph"></param>
        /// <param name="instance"></param>
        /// <param name="lifetime"></param>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="instance"/> is an invalid type</exception>
        /// <exception cref="ArgumentNullException"><paramref name="instantiationGraph"/>
        /// and/or <paramref name="instance"/> is <see langword="null" />.</exception>
        public void Build(ServiceInstantiationGraph instantiationGraph, object instance, AbstractLifeTime lifetime)
        {
            if (instantiationGraph == null)
            {
                throw new ArgumentNullException(nameof(instantiationGraph));
            }
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }
            var convertedInstance = instance as TConcreteType;

            if (convertedInstance == null)
            {
                throw new InvalidOperationException(
                        $"Instance is of a different type [required:{typeof(TConcreteType)}, supplied:{instance.GetType()}]");
            }
            foreach (var propSetter in _propSetters)
            {
                if (!instantiationGraph.IsTypeSupported(propSetter.Item1))
                    continue;

                var propValue = instantiationGraph.Get(propSetter.Item1, lifetime);
                propSetter.Item2(convertedInstance, propValue);
            }
        }
    }
}