namespace Nyx.Composition.Impl
{
    internal abstract class AbstractLifeTime : ILifetime
    {
        public abstract void Dispose();
        public abstract void Register(object instance);
    }
}