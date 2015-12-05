using System;
using Nyx.AppSupport.AppServices;
using Nyx.AppSupport.SystemTray;

namespace Nyx.AppSupport
{
    public static class NyxApplicationExtensions
    {
        public static void UsesSystemTray(this INyxApplicationConfiguration app, Action<ISystemTrayServiceConfigurator> configurator)
        {
            if (configurator == null)
            {
                throw new ArgumentNullException(nameof(configurator));
            }

            app.Register<ISystemTrayService>()
                .UsingConcreteType<SystemTrayService>()
                .AsSingleton()
                .InitializeWith(x => configurator(x.Configure()));
        }

        public static void UsesNotificationServices(this INyxApplicationConfiguration app)
        {
            app.Register<MessageBoxNotificationService>();
            app.Register<SystemTrayService>();
        }

        public static void UsesNotificationServices<TDefaultService>(this INyxApplicationConfiguration app)
            where TDefaultService : class, IUserNotificationService
        {
            app.Register<MessageBoxNotificationService>();
            app.Register<SystemTrayService>();

            app.Register<IUserNotificationService>().UsingConcreteType<TDefaultService>();
        }
    }
}