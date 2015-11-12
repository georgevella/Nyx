using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Nyx.Presentation.Conventions
{
    /// <summary>
    /// </summary>
    public class ByNameViewModelDiscoveryConvention : IViewModelDiscoveryConvention
    {

        public void FindAndRegisterMappings(IViewResolver viewResolver, Assembly assembly)
        {
            var filter = assembly.DefinedTypes
                .Where(t => !t.IsInterface && t.IsClass && !t.IsAbstract)
                .Where(t => t.Name.EndsWith("ViewModel") || t.Name.EndsWith("View") || t.Name.EndsWith("Window"));

            var viewModelMap = new Dictionary<string, Type>();
            var viewMap = new Dictionary<string, Type>();

            foreach (var type in filter)
            {
                var name = type.Name;
                if (name.EndsWith("ViewModel"))
                {
                    name = name.Remove(name.Length - "ViewModel".Length);
                    viewModelMap[name] = type.AsType();
                    continue;
                }

                if (name.EndsWith("View"))
                {
                    name = name.Remove(name.Length - "View".Length);
                    viewMap[name] = type.AsType();
                    continue;
                }

                if (name.EndsWith("Window"))
                {
                    name = name.Remove(name.Length - "Window".Length);
                    viewMap[name] = type.AsType();
                    continue;
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
                viewResolver.AddMapping(mapping.ViewModelType, mapping.ViewType);
            }
        }
    }
}