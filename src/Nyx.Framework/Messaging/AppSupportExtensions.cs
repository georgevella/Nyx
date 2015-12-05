using System;
using Nyx.AppSupport;

namespace Nyx.Messaging
{
    public static class AppSupportExtensions
    {
        public static void UsesMessageRouter(this INyxApplicationConfiguration c, Action<IMessageRouterConfigurator> config)
        {
            var configuration = new MessageRouterConfiguration();
            config(configuration);

            c.Register<IMessageRouter>()
                .UsingConcreteType<MessageRouter>()
                .AsSingleton()
                .InitializeWith(x => ((MessageRouter)x).ConfigureWith(configuration));

            configuration.Mappings.ForEach(m => c.Register(m.MessageType));
            configuration.Handlers.ForEach(h => c.Register(h));
        }
    }
}