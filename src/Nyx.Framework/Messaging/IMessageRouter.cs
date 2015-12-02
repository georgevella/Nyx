namespace Nyx.Messaging
{
    public interface IMessageRouter
    {
        void Register<TMessage, THandler>()
            where THandler : IMessageHandler<TMessage>
            where TMessage : class;
    }
}