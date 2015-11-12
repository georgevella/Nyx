using System;
using System.Linq;
using FluentAssertions;
using Nyx.Composition.Tests.Stubs;
using Xunit;

namespace Nyx.Composition.Tests
{
    public class ServiceResolutionTests
    {
        [Fact]
        public void RequestedServiceShouldReturnInstanceOfConfiguredConcreteType()
        {
            var pyxis = ContainerFactory.Setup(c =>
            {
                c.Register<IStub>().UsingConcreteType<Stub>();
            });

            var testobject = pyxis.Get<IStub>();

            testobject.Should().BeOfType<Stub>();
        }

        [Fact]
        public void ContainerShouldResolveConstructorDependencies()
        {
            var pyxis = ContainerFactory.Setup(c =>
            {
                c.Register<IStub>().UsingConcreteType<Stub2>();
                c.Register<IStubDependency>().UsingConcreteType<StubDependency>();
            });

            var testobject = pyxis.Get<IStub>();

            testobject.Should().BeOfType<Stub2>();

            var stub2 = (Stub2)testobject;
            stub2.Dependency.Should().NotBeNull();
        }

        [Fact]
        public void ContainerShouldFailInitializationIfAConstructorDependencyIsMissing()
        {
            Action a = () => ContainerFactory.Setup(c =>
            {
                c.Register<IStub>().UsingConcreteType<Stub2>();

            });

            a.ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void ContainerShouldNotFailInitializationIfADependencyIsMissingButOptional()
        {
            IStub testobject = null;

            Action a = () =>
            {
                var pyxis = ContainerFactory.Setup(c =>
                {
                    c.Register<IStub>().UsingConcreteType<Stub2WithOptionalParameter>();

                });

                testobject = pyxis.Get<IStub>();
            };

            a.ShouldNotThrow();

            testobject.Should().BeOfType<Stub2WithOptionalParameter>();
            ((Stub2WithOptionalParameter)testobject).Dependency.Should().BeNull();
        }

        [Fact]
        public void ContainerShouldChooseMostSuitableConstructor()
        {
            var pyxis = ContainerFactory.Setup(c =>
            {
                c.Register<IStub>().UsingConcreteType<StubWithMultipleCtors>();
                c.Register<IStubDependency>().UsingConcreteType<StubDependency>();
                c.Register<IAnotherDependency>().UsingConcreteType<AnotherDependency>();
            });

            var testobject = pyxis.Get<IStub>();

            testobject.Should().BeOfType<StubWithMultipleCtors>();

            var stub2 = (StubWithMultipleCtors)testobject;
            stub2.Dependency.Should().NotBeNull().And.BeOfType<StubDependency>();
            stub2.AnotherDependency.Should().NotBeNull().And.BeOfType<AnotherDependency>();
        }

        [Fact]
        public void OptionalParametersInConstructorsShouldBeHandledProperly()
        {
            var pyxis = ContainerFactory.Setup(c =>
            {
                c.Register<IStub>().UsingConcreteType<StubWithMultipleCtorsAndOptionalParameter>();
                c.Register<IStubDependency>().UsingConcreteType<StubDependency>();
            });

            var testobject = pyxis.Get<IStub>();

            testobject.Should().BeOfType<StubWithMultipleCtorsAndOptionalParameter>();

            var stub2 = (StubWithMultipleCtorsAndOptionalParameter)testobject;
            stub2.Dependency.Should().NotBeNull().And.BeOfType<StubDependency>();
            stub2.AnotherDependency.Should().BeNull();
        }

        [Fact]
        public void SetupMethodShouldSupportOnlyInterfaces()
        {
            Action a = () => ContainerFactory.Setup(
                c =>
                {
                    c.Register<Stub>();
                });

            a.ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void ContainerShouldInjectAllServicableProperties()
        {
            var pyxis = ContainerFactory.Setup(c =>
            {
                c.Register<IStub>().UsingConcreteType<StubWithPropInjection>();
                c.Register<IStubDependency>().UsingConcreteType<StubDependency>();
                c.Register<IAnotherDependency>().UsingConcreteType<AnotherDependencyDependingOnStubDependency>();
            });

            var testobject = pyxis.Get<IStub>();
            testobject.Should().BeOfType<StubWithPropInjection>();

            var obj = (StubWithPropInjection)testobject;
            obj.Dependency.Should().NotBeNull().And.BeOfType<AnotherDependencyDependingOnStubDependency>();

            var dep = (AnotherDependencyDependingOnStubDependency)obj.Dependency;
            obj.StubDependency.Should().NotBeNull()
                .And.BeOfType<StubDependency>()
                .And.BeSameAs(dep.StubDependency);
        }

        [Fact]
        public void ContainerShouldCallConfiguredInitializationMethod()
        {
            var pyxis = ContainerFactory.Setup(c =>
            {
                c.Register<IStub>().UsingConcreteType<DisposableStub>().InitializeWith(x => x.Initializer());
            });

            throw new NotImplementedException();
        }

        [Fact]
        public void ContainerShouldAllowMultipleRegistrationOfNamedTypes()
        {
            var pyxis = ContainerFactory.Setup(c =>
            {
                c.Register<IStub>().UsingConcreteType<NamedStub>().Named("stub1");
                c.Register<IStub>().UsingConcreteType<DisposableStub>().Named("stub2");
            });

            var stub = pyxis.Get<IStub>("stub1");
            stub.Should().BeOfType<NamedStub>();
            stub = pyxis.Get<IStub>("stub2");
            stub.Should().BeOfType<DisposableStub>();
        }

        [Fact]
        public void ContainerShouldAllowRegistrationOfSingletonTypesUsingProvidedInstance()
        {
            var pyxis = ContainerFactory.Setup(c =>
            {
                c.Register(typeof(IStub)).Using(typeof(Stub));
            });

            IStub stub = null;

            Action a = () => stub = pyxis.Get<IStub>();

            a.ShouldNotThrow();
            stub.Should().NotBeNull().And.BeOfType<Stub>();

        }
    }
}
