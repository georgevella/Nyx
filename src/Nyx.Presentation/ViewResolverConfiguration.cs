using System;
using System.Collections.Generic;

namespace Nyx.Presentation
{
    internal class ViewResolverConfiguration : IViewResolverConfiguration
    {
        private readonly Dictionary<Type, Type> _mappings;

        internal ViewResolverConfiguration(Dictionary<Type, Type> mappings)
        {
            if (mappings == null)
                throw new ArgumentNullException(nameof(mappings));

            _mappings = mappings;
        }

        public virtual void AddMapping<TViewModel, TView>()
            where TViewModel : IViewModel, new() where TView : new()
        {
            _mappings[typeof(TViewModel)] = typeof(TView);
        }

        public virtual void AddMapping(Type viewModelType, Type viewType)
        {
            _mappings[viewModelType] = viewType;
        }

        public void AutoDiscoverUsing(IViewModelDiscoveryConvention viewModelDiscoveryConvention)
        {
            viewModelDiscoveryConvention.FindAndRegisterMappings(this);
        }
    }
}