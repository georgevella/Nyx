using System;

namespace Nyx.Messaging
{
    public class UnsupportedMessageType : Exception
    {
        public UnsupportedMessageType(Type type) : base($"Message {type} is not supported")
        {

        }
    }
}