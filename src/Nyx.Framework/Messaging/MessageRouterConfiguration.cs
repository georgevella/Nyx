using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Nyx.Messaging
{
    internal class MessageRegistration
    {
        public MessageRegistration(Type messageType, Type handlerType)
        {
            MessageType = messageType;
            HandlerType = handlerType;
        }

        public Type MessageType { get; }

        public Type HandlerType { get; }
    }

    class MessageRouterConfiguration : IMessageRouterConfigurator
    {
        private readonly List<MessageRegistration> _mappings = new List<MessageRegistration>();
        private readonly List<Type> _handlers = new List<Type>();

        internal IEnumerable<MessageRegistration> Mappings => _mappings;

        public List<Type> Handlers => _handlers;

        public MessageRouterConfiguration()
        {

        }


        public void Register<TMessage, THandler>() where TMessage : class where THandler : IMessageHandler<TMessage>
        {
            Register(typeof(TMessage), typeof(THandler));
        }

        private void Register(Type messageType, Type handlerType)
        {
            _mappings.Add(new MessageRegistration(messageType, handlerType));
            if (!_handlers.Contains(handlerType))
                _handlers.Add(handlerType);
        }

        public void AutoWireEverything(Assembly assembly)
        {
            var registrarMethod = GetType().GetTypeInfo().DeclaredMethods.First(x => x.Name == nameof(Register));

            foreach (var handlerType in assembly.ExportedTypes)
            {
                var supportedMessages = handlerType.GetTypeInfo().ImplementedInterfaces
                    .Where(
                        intf =>
                            intf.IsConstructedGenericType &&
                            intf.GetGenericTypeDefinition() == typeof(IMessageHandler<>))
                    .Select(intf => intf.GenericTypeArguments[0])
                    .ToList();

                if (supportedMessages.Count == 0)
                    continue;

                supportedMessages.ForEach(mt => Register(mt, handlerType));
            }
        }
    }
}