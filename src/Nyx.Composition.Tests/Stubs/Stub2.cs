namespace Nyx.Composition.Tests.Stubs
{
    public class Stub2 : IStub
    {
        internal readonly IStubDependency Dependency;

        public Stub2(IStubDependency dependency)
        {
            Dependency = dependency;
        }

        public void Initializer()
        {
            throw new System.NotImplementedException();
        }
    }
}