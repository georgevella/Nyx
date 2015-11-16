using System;
using System.Collections;
using FluentAssertions;
using Nyx.Composition.Tests.Stubs;
using Xunit;

namespace Nyx.Composition.Tests
{
    // ReSharper disable once TestFileNameWarning
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
            var container = ContainerFactory.Setup(c =>
            {
                c.Register<IStub>().UsingConcreteType<DisposableStub>().AsSingleton();

                c.Register<Stub2>().AsSingleton();
            });

            container.Get<IStub>().Should().BeSameAs(container.Get<IStub>());
            container.Get<Stub2>().Should().BeSameAs(container.Get(typeof(Stub2)));
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