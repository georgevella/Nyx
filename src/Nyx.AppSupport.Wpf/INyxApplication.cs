using System.Reflection;
using Nyx.Composition;

namespace Nyx.AppSupport.Wpf
{
    public interface INyxApplication : IContainerConfiguration
    {
        INyxApplication UsingDefaultConventions();
        INyxApplication AutoDiscoverViewModels(Assembly assembly);
        INyxApplication AutoDiscoverViewModels();
    }
}