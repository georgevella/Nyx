namespace Nyx.Tests.Stubs
{
    public class Stub2WithOptionalParameter : IStub
    {
        internal readonly IStubDependency Dependency;

        public Stub2WithOptionalParameter(IStubDependency dependency = null)
        {
            Dependency = dependency;
        }

        public void Initializer()
        {
            throw new System.NotImplementedException();
        }
    }
}