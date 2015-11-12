namespace Nyx.Composition.Tests.Stubs
{
    public class StubWithDependency : IStub
    {
        internal readonly IStubDependency Dependency;

        public StubWithDependency(IStubDependency dependency)
        {
            Dependency = dependency;
        }

        public void Initializer()
        {
            throw new System.NotImplementedException();
        }
    }
}