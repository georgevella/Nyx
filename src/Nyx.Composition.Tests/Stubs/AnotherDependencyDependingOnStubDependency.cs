namespace Nyx.Composition.Tests.Stubs
{
    class AnotherDependencyDependingOnStubDependency : IAnotherDependency
    {
        public IStubDependency StubDependency { get; set; }
    }
}