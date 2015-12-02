namespace Nyx.Tests.Stubs
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

    public class StubWithDependency2 : IStub
    {
        public StubDependency Dependency { get; }

        public StubWithDependency2(StubDependency dependency)
        {
            Dependency = dependency;
        }

        public void Initializer()
        {
            throw new System.NotImplementedException();
        }
    }
}