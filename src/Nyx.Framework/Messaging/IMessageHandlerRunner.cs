namespace Nyx.Messaging
{
    internal interface IMessageHandlerRunner
    {
        void Invoke(object message);
    }
}