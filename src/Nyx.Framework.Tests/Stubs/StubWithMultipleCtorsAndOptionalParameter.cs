namespace Nyx.Tests.Stubs
{
    class StubWithMultipleCtorsAndOptionalParameter : IStub
    {
        public IStubDependency Dependency { get; }
        public IAnotherDependency AnotherDependency { get; }

        public StubWithMultipleCtorsAndOptionalParameter() : this(new StubDependency(), new AnotherDependency())
        {

        }

        public StubWithMultipleCtorsAndOptionalParameter(IStubDependency stubDependency, IAnotherDependency anotherDependency = null)
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