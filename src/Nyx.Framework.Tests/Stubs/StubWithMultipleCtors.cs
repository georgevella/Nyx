namespace Nyx.Tests.Stubs
{
    class StubWithMultipleCtors : IStub
    {
        public IStubDependency Dependency { get; }
        public IAnotherDependency AnotherDependency { get; }

        public StubWithMultipleCtors(IStubDependency stubDependency) : this(stubDependency, null)
        {

        }

        public StubWithMultipleCtors(IStubDependency stubDependency, IAnotherDependency anotherDependency)
        {
            Dependency = stubDependency;
            AnotherDependency = anotherDependency;
        }

        public void Initializer()
        {
            throw new System.NotImplementedException();
        }
    }
}