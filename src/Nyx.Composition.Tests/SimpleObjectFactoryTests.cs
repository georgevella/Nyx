using System;
using FluentAssertions;
using Nyx.Composition.ServiceFactories;
using Nyx.Composition.Tests.Stubs;
using Xunit;

namespace Nyx.Composition.Tests
{
    public class SimpleObjectFactoryTests
    {
        [Fact]
        public void CreatesInstancesOfType()
        {
            var builder = new SimpleServiceFactory(typeof(Stub), typeof(Stub).GetConstructor(new Type[] { }));

            builder.Create(null).Should().BeOfType<Stub>();
        }
    }
}