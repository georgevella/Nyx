namespace Nyx.Messaging
{
    public interface IMessageRouter
    {
        void Post(object message);
    }
}