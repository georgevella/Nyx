using System;
using System.Collections;
using System.Collections.Generic;

namespace Nyx.Presentation
{
    public abstract class BaseViewResolver : IViewResolver, IEnumerable<KeyValuePair<Type, Type>>
    {
        private readonly Dictionary<Type, Type> _viewModelToViewMapping = new Dictionary<Type, Type>();

        public Type ResolveViewTypeFor(Type viewModelType)
        {
            return _viewModelToViewMapping[viewModelType];
        }

        public Type ResolveViewTypeFor<TViewModel>()
        {
            return _viewModelToViewMapping[typeof(TViewModel)];
        }

        protected abstract IViewResolverConfiguration BuildResolverConfiguration(Dictionary<Type, Type> mappings);

        public virtual void Setup(Action<IViewResolverConfiguration> c)
        {
            var vrc = BuildResolverConfiguration(_viewModelToViewMapping);

            c(vrc);
        }

        public IEnumerator<KeyValuePair<Type, Type>> GetEnumerator()
        {
            return _viewModelToViewMapping.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_viewModelToViewMapping).GetEnumerator();
        }
    }
}