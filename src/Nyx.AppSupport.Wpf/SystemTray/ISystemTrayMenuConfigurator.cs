using System;

namespace Nyx.AppSupport.SystemTray
{
    public interface ISystemTrayMenuConfigurator
    {
        ISystemTrayMenuConfigurator MenuItem<T>(string text)
            where T : class, new();
        ISystemTrayMenuConfigurator MenuItem<T>(string text, Uri iconUri)
            where T : class, new();
        ISystemTrayMenuConfigurator Seperator();
    }
}