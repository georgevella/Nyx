using System;

namespace Nyx.Presentation
{
    public interface IViewResolverConfiguration
    {
        void AddMapping<TViewModel, TView>()
            where TViewModel : IViewModel, new()
            where TView : new();

        void AddMapping(Type viewModelType, Type viewType);

        void AutoDiscoverUsing(IViewModelDiscoveryConvention viewModelDiscoveryConvention);
    }
}