using System;
using System.Collections.Generic;
using Nyx.Composition;

namespace Nyx.Messaging
{
    public class MessageRouter : IMessageRouter
    {
        private readonly IContainer _container;
        private readonly Dictionary<Type, List<Action<dynamic>>> _messageHandlerRunners = new Dictionary<Type, List<Action<dynamic>>>();

        public MessageRouter(IContainer container)
        {
            _container = container;
        }

        public void Register<TMessage, THandler>() where TMessage : class where THandler : IMessageHandler<TMessage>
        {
            List<Action<dynamic>> handlers = null;
            if (!_messageHandlerRunners.TryGetValue(typeof(TMessage), out handlers))
            {
                handlers = _messageHandlerRunners[typeof(TMessage)] = new List<Action<dynamic>>();
            }

            handlers.Add((m) =>
            {
                var handler = _container.Get<THandler>();
                handler.Handle((TMessage)m);

            });
        }

        public void Post<TMessage>(TMessage message)
            where TMessage : class
        {
            List<Action<dynamic>> handlers;
            if (!_messageHandlerRunners.TryGetValue(typeof(TMessage), out handlers))
            {
                throw new InvalidOperationException("Message unknown");
            }

            try
            {
                foreach (var handler in handlers)
                {
                    handler(message);
                }
            }
            catch (Exception e)
            {

            }
        }
    }
}