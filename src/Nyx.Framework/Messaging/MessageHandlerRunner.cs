using Nyx.Composition;

namespace Nyx.Messaging
{
    class MessageHandlerRunner<TMessage, THandler> : IMessageHandlerRunner
        where TMessage : class where THandler : IMessageHandler<TMessage>
    {
        private readonly IContainer _container;

        public void Invoke(object message)
        {
            var handler = _container.Get<THandler>();
            handler.Handle((TMessage)message);
        }

        public MessageHandlerRunner(IContainer container)
        {
            _container = container;
        }
    }
}