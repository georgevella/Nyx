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

            using (var lifecycle = pyxis.UsingLifetimeService())
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


        [Fact]
        public void ContainerShouldReturnDifferentInstancesWhenServiceIsTransient()
        {
            var container = ContainerFactory.Setup(c =>
            {
                c.Register<IStub>().UsingConcreteType<TransientTestStub>();
                c.Register<IStubDependency>().UsingConcreteType<StubDependency>().AsTransient();
            });

            IStub stub = null;

            Action a = () => stub = container.Get<IStub>();

            a.ShouldNotThrow();
            stub.Should().NotBeNull().And.BeOfType<TransientTestStub>();

            var target = (TransientTestStub)stub;

            target.Dependency1.Should().NotBeSameAs(target.Dependency2);
        }
        [Fact]
        public void ContainerShouldReturnSameInstancesWhenServiceIsNotTransient()
        {
            var container = ContainerFactory.Setup(c =>
            {
                c.Register<IStub>().UsingConcreteType<TransientTestStub>();
                c.Register<IStubDependency>().UsingConcreteType<StubDependency>();
            });

            IStub stub = null;

            Action a = () => stub = container.Get<IStub>();

            a.ShouldNotThrow();
            stub.Should().NotBeNull().And.BeOfType<TransientTestStub>();

            var target = (TransientTestStub)stub;

            target.Dependency1.Should().BeSameAs(target.Dependency2);
        }
    }
}