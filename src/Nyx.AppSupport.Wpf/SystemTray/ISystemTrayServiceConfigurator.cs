using System;

namespace Nyx.AppSupport.SystemTray
{
    public interface ISystemTrayServiceConfigurator
    {
        ISystemTrayServiceConfigurator SetIcon(Uri packUri);
        ISystemTrayServiceConfigurator RightclickMenu(Action<ISystemTrayMenuConfigurator> c);
    }
}