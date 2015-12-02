namespace Nyx.Tests.Stubs
{
    public class TransientTestStub : IStub
    {
        public IStubDependency Dependency1 { get; set; }
        public IStubDependency Dependency2 { get; set; }

        public TransientTestStub(IStubDependency dependency1, IStubDependency dependency2)
        {
            Dependency1 = dependency1;
            Dependency2 = dependency2;
        }

        public void Initializer()
        {

        }
    }
}