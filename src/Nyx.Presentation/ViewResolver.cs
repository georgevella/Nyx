using System;
using System.Collections;
using System.Collections.Generic;

namespace Nyx.Presentation
{
    public class ViewResolver : IViewResolver, IEnumerable<KeyValuePair<Type, Type>>
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

        public void AddMapping<TViewModel, TView>()
            where TViewModel : class, IViewModel
            where TView : class
        {
            AddMapping(typeof(TViewModel), typeof(TView));
        }

        public void AddMapping(Type viewModelType, Type viewType)
        {
            _viewModelToViewMapping.Add(viewModelType, viewType);
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