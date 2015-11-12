using System;
using System.Collections.Generic;

namespace Nyx.Presentation
{
    public class ViewResolver : BaseViewResolver
    {
        protected override IViewResolverConfiguration BuildResolverConfiguration(Dictionary<Type, Type> mappings)
        {
            var vrc = new ViewResolverConfiguration(mappings);
            return vrc;

        }
    }
}