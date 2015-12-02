namespace Nyx.Tests.Stubs
{
    class AnotherDependencyDependingOnStubDependency : IAnotherDependency
    {
        public IStubDependency StubDependency { get; set; }
    }
}