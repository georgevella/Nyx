namespace Nyx.Tests.Stubs
{
    public class StubWithPropInjection : IStub
    {
        public IAnotherDependency Dependency { get; set; }

        public IStubDependency StubDependency { get; set; }
        public void Initializer()
        {
            throw new System.NotImplementedException();
        }
    }
}