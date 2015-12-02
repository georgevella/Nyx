using System;

namespace Nyx.Presentation
{
    public interface IViewResolver
    {
        Type ResolveViewTypeFor(Type viewModelType);
        Type ResolveViewTypeFor<TViewModel>();

        void AddMapping<TViewModel, TView>()
            where TViewModel : class, IViewModel
            where TView : class;

        void AddMapping(Type viewModelType, Type viewType);
    }
}