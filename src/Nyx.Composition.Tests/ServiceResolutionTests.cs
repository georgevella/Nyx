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
                c.Register<IStub>().UsingConcreteType<StubWithDependency>();
                c.Register<IStubDependency>().UsingConcreteType<StubDependency>();
            });

            var testobject = pyxis.Get<IStub>();

            testobject.Should().BeOfType<StubWithDependency>();

            var stub2 = (StubWithDependency)testobject;
            stub2.Dependency.Should().NotBeNull();
        }

        [Fact]
        public void ContainerShouldFailInitializationIfAConstructorDependencyIsMissing()
        {
            Action a = () => ContainerFactory.Setup(c =>
            {
                c.Register<IStub>().UsingConcreteType<StubWithDependency>();

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

            var stub = pyxis.Get<IStub>();

            stub.Should().NotBeNull().And.BeOfType<DisposableStub>();
            stub.As<DisposableStub>().InitializerCalled.Should().BeTrue();
        }

        [Fact]
        public void ContainerShouldAllowMultipleRegistrationOfNamedTypes()
        {
            var pyxis = ContainerFactory.Setup(c =>
            {
                c.Register<IStub>().UsingConcreteType<AnotherStub>().Named("stub1");
                c.Register<IStub>().UsingConcreteType<DisposableStub>().Named("stub2");
            });

            var stub = pyxis.Get<IStub>("stub1");
            stub.Should().BeOfType<AnotherStub>();
            stub = pyxis.Get<IStub>("stub2");
            stub.Should().BeOfType<DisposableStub>();
        }

        [Fact]
        public void ContainerShouldAllowRegistrationOfSingletonTypesUsingProvidedInstance()
        {
            var pyxis = ContainerFactory.Setup(c =>
            {
                c.Register(typeof(IStub)).UsingConcreteType(typeof(Stub));
            });

            IStub stub = null;

            Action a = () => stub = pyxis.Get<IStub>();

            a.ShouldNotThrow();
            stub.Should().NotBeNull().And.BeOfType<Stub>();
        }

        [Fact]
        public void RegisteringAConcreteServiceWithAConcreteSubType()
        {
            var pyxis = ContainerFactory.Setup(c =>
            {
                c.Register<Stub>().UsingConcreteType<Stub2>();
            });

            IStub stub = null;

            Action a = () => stub = pyxis.Get<Stub>();

            a.ShouldNotThrow();
            stub.Should().NotBeNull().And.BeOfType<Stub2>();
        }


        [Fact]
        public void RegisteringAConcreteServiceWithAConcreteSubType_Dependencies()
        {
            var pyxis = ContainerFactory.Setup(c =>
            {
                c.Register<IStub>().UsingConcreteType<StubWithDependency2>();
                c.Register<StubDependency>().UsingConcreteType<ChildStubDependency>();
            });

            IStub stub = null;

            Action a = () => stub = pyxis.Get<IStub>();

            a.ShouldNotThrow();
            stub.Should().NotBeNull().And.BeOfType<StubWithDependency2>();
            var target = (StubWithDependency2)stub;
            target.Dependency.Should().NotBeNull().And.BeOfType<ChildStubDependency>();
        }

        [Fact]
        public void RegisteringAConcreteServiceWithStaticTargetShouldAlwaysReturnSameInstance()
        {
            var stubDependency = new ChildStubDependency();

            var pyxis = ContainerFactory.Setup(c =>
            {
                c.Register<IStub>().UsingConcreteType<StubWithDependency2>();
                c.Register<StubDependency>().Using(stubDependency);
            });

            IStub stub = null, stub2 = null;

            Action a = () => stub = pyxis.Get<IStub>();

            a.ShouldNotThrow();

            stub2 = stub;
            a.ShouldNotThrow();

            stub.Should().NotBeNull().And.BeOfType<StubWithDependency2>();
            stub2.Should().NotBeNull().And.BeOfType<StubWithDependency2>();

            stub2.Should().NotBeSameAs(stub);

            var target1 = (StubWithDependency2)stub;
            target1.Dependency.Should().NotBeNull().And.BeSameAs(stubDependency);

            var target2 = (StubWithDependency2)stub;
            target2.Dependency.Should().NotBeNull().And.BeSameAs(stubDependency);

            target1.Dependency.Should().BeSameAs(target2.Dependency);
        }
    }
}
