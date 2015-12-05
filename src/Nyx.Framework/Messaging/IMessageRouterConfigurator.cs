using System.Reflection;

namespace Nyx.Messaging
{
    public interface IMessageRouterConfigurator
    {
        void Register<TMessage, THandler>()
            where THandler : IMessageHandler<TMessage>
            where TMessage : class;

        void AutoWireEverything(Assembly assembly);
    }
}