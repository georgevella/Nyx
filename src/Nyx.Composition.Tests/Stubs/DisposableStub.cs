using System;

namespace Nyx.Composition.Tests.Stubs
{
    public class DisposableStub : IStub, IDisposable
    {
        public bool Disposed { get; private set; }

        public void Dispose()
        {
            Disposed = true;
        }

        public void Initializer()
        {
            throw new NotImplementedException();
        }
    }
}