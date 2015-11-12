using System;
using FluentAssertions;
using Nyx.Composition.Tests.Stubs;
using Xunit;

namespace Nyx.Composition.Tests
{
    public class LifecycleSupportTests
    {

        [Fact]
        public void ServicesShouldBeProperlyDisposedAtTheEndOfTheirLifecycle()
        {
            var pyxis = ContainerFactory.Setup(c =>
            {
                c.Register<IStub>().UsingConcreteType<DisposableStub>();
            });

            IStub obj = null;

            using (var lifecycle = pyxis.BeginLifecycle())
            {
                obj = pyxis.Get<IStub>(lifecycle);
            }

            obj.Should().NotBeNull()
                .And.BeOfType<DisposableStub>();
            ((DisposableStub)obj).Disposed.Should().BeTrue();
        }

        [Fact]
        public void ServicesMarkedAsSingletonShouldBeReused()
        {
            var pyxis = ContainerFactory.Setup(c =>
            {
                c.Register<IStub>().UsingConcreteType<DisposableStub>().AsSingleton();
            });

            var a = pyxis.Get<IStub>();
            var b = pyxis.Get<IStub>();

            a.Should().BeSameAs(b);
        }
    }
}