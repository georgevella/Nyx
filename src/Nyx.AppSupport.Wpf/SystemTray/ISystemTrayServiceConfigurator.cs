using System;

namespace Nyx.AppSupport.SystemTray
{
    public interface ISystemTrayServiceConfigurator
    {
        ISystemTrayServiceConfigurator UsingIcon(Uri packUri);
        ISystemTrayServiceConfigurator Menu(Action<ISystemTrayMenuConfigurator> c);
    }
}