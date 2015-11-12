using System.Reflection;

namespace Nyx.Presentation
{
    public interface IViewModelDiscoveryConvention
    {
        void FindAndRegisterMappings(IViewResolver viewResolver, Assembly assembly);
    }
}