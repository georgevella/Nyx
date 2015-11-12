using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Nyx.Presentation.Conventions
{
    public class ByNameViewModelDiscoveryConvention : IViewModelDiscoveryConvention
    {
        private readonly Assembly _assembly;

        public ByNameViewModelDiscoveryConvention(Assembly assembly)
        {
            _assembly = assembly;
        }

        public void FindAndRegisterMappings(IViewResolverConfiguration vrc)
        {
            var filter = _assembly.DefinedTypes
                .Where(t => !t.IsInterface && t.IsClass && !t.IsAbstract)
                .Where(t => t.Name.EndsWith("ViewModel") || t.Name.EndsWith("View"));

            var viewModelMap = new Dictionary<string, Type>();
            var viewMap = new Dictionary<string, Type>();
            foreach (var type in filter)
            {
                var name = type.Name;
                if (name.EndsWith("ViewModel"))
                {
                    name = name.Remove(name.Length - "ViewModel".Length);
                    viewModelMap[name] = type.AsType();
                }
                else if (name.EndsWith("View") || name.EndsWith("Pane"))
                {
                    name = name.Remove(name.Length - 4);
                    viewMap[name] = type.AsType();
                }
            }

            var mappings = from item in viewModelMap
                           let vmKey = item.Key
                           where viewMap.ContainsKey(vmKey)
                           let vType = viewMap[vmKey]
                           let vmType = item.Value
                           select new
                           {
                               ViewType = vType,
                               ViewModelType = vmType
                           };

            foreach (var mapping in mappings)
            {
                vrc.AddMapping(mapping.ViewModelType, mapping.ViewType);
            }
        }
    }
}