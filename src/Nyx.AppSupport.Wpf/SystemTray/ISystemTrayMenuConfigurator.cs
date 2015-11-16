using System;

namespace Nyx.AppSupport.SystemTray
{
    public interface ISystemTrayMenuConfigurator
    {
        ISystemTrayMenuConfigurator MenuItem(string text, object messageId);
        ISystemTrayMenuConfigurator MenuItem(string text, Uri packUri, object messageId);
        ISystemTrayMenuConfigurator Seperator();
    }
}