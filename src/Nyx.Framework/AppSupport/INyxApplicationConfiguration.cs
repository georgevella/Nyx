using System.Reflection;
using Nyx.Composition;

namespace Nyx.AppSupport
{
    public interface INyxApplicationConfiguration : IContainerConfiguration
    {
        INyxApplicationConfiguration UsingDefaultConventions();
        INyxApplicationConfiguration AutoDiscoverViewModels(Assembly assembly);
        INyxApplicationConfiguration AutoDiscoverViewModels();
        INyxApplicationConfiguration AutoDiscoverCommands(Assembly assembly);
        INyxApplicationConfiguration AutoDiscoverCommands();
    }
}