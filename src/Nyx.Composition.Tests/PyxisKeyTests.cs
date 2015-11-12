using FluentAssertions;
using Nyx.Composition.Tests.Stubs;
using Xunit;

namespace Nyx.Composition.Tests
{
    public class PyxisKeyTests
    {
        [Fact]
        public void DifferentObjectsWithSameValuesShouldBeEqual()
        {
            var key1 = new ServiceKey(typeof(IStub));
            var key2 = new ServiceKey(typeof(IStub));

            key2.Should().Be(key1);
        }
    }
}