using System;
using System.Collections.Generic;
using System.Reflection;
using Nyx.Presentation.Conventions;

namespace Nyx.Presentation
{
    public interface IDiscoveryConvention
    {
        IEnumerable<KeyValuePair<Type, Type>> DiscoverViewModelsAndViews(Assembly assembly);

        IEnumerable<Type> DiscoverCommands(Assembly assembly);
    }
}