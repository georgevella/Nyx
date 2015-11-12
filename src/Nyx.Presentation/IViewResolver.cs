using System;

namespace Nyx.Presentation
{
    public interface IViewResolver
    {
        Type ResolveViewTypeFor(Type viewModelType);
        Type ResolveViewTypeFor<TViewModel>();
        void Setup(Action<IViewResolverConfiguration> c);
    }

    public interface IViewModelDiscoveryConvention
    {
        void FindAndRegisterMappings(IViewResolverConfiguration vrc);
    }

}