using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Nyx.Composition;

namespace Nyx.Messaging
{
    public class MessageRouter : IMessageRouter
    {
        private readonly IContainer _container;
        private readonly Dictionary<Type, List<Action<dynamic>>> _messageHandlerInvokers = new Dictionary<Type, List<Action<dynamic>>>();

        public MessageRouter(IContainer container)
        {
            _container = container;
        }

        internal void ConfigureWith(MessageRouterConfiguration configuration)
        {
            var addHandlerInvokerMethod = GetType().GetTypeInfo().DeclaredMethods.First(x => x.Name == nameof(AddHandlerInvoker));

            foreach (var mapping in configuration.Mappings)
            {
                addHandlerInvokerMethod.MakeGenericMethod(mapping.MessageType, mapping.HandlerType).Invoke(this, null);
            }
        }

        public void AddHandlerInvoker<TMessage, THandler>() where TMessage : class where THandler : IMessageHandler<TMessage>
        {
            List<Action<dynamic>> handlers = null;
            if (!_messageHandlerInvokers.TryGetValue(typeof(TMessage), out handlers))
            {
                handlers = _messageHandlerInvokers[typeof(TMessage)] = new List<Action<dynamic>>();
            }

            handlers.Add((m) =>
            {
                var handler = _container.Get<THandler>();
                handler.Handle((TMessage)m);

            });
        }

        public void Post(object message)
        {
            var type = message.GetType();
            List<Action<dynamic>> runners;
            if (_messageHandlerInvokers.TryGetValue(type, out runners))
            {
                foreach (var r in runners)
                    r(message);

                return;
            }

            throw new UnsupportedMessageType(type);
        }
    }
}