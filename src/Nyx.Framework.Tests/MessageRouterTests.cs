using System.Reflection;
using Nyx.Messaging;
using Xunit;
using System.Linq;
using FluentAssertions;

namespace Nyx.Tests
{
    public class MessageRouterTests
    {
        [Fact]
        public void Test()
        {
            var type = typeof(Other).GetTypeInfo();

            var supportedMessages =
                type.ImplementedInterfaces.Where(intf => intf.GetGenericTypeDefinition() == typeof(IMessageHandler<>))
                    .Select(intf => intf.GenericTypeArguments[0])
                    .ToList();

        }
    }

    class Message
    {

    }
    class Message2
    {

    }

    class Other
    {

    }

    class TestMessageHandler : IMessageHandler<Message>, IMessageHandler<Message2>
    {
        public void Handle(Message message)
        {
            throw new System.NotImplementedException();
        }

        public void Handle(Message2 message)
        {
            throw new System.NotImplementedException();
        }
    }
}