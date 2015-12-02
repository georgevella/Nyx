using FluentAssertions;
using Nyx.Composition;
using Nyx.Tests.Stubs;
using Xunit;

namespace Nyx.Tests
{
    public class PyxisKeyTests
    {
        [Fact]
        public void DifferentObjectsWithSameValuesShouldBeEqual()
        {
            var key1 = new ServiceKey(typeof(IStub));
            var key2 = new ServiceKey(typeof(IStub));

            AssertionExtensions.Should((object) key2).Be(key1);
        }
    }
}