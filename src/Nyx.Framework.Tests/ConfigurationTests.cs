using FluentAssertions;
using Nyx.Composition.Impl;
using Nyx.Tests.Stubs;
using Xunit;

namespace Nyx.Tests
{
    public class ConfigurationTests
    {
        [Fact]
        public void ConfigurationShouldStoreRegistration()
        {
            var cfg = new FluentContainerConfigurator();
            var reg = cfg.Register<IStub>();

            cfg.Registrations.Should().Contain(x => x.Equals(reg));
        }

        [Fact]
        public void ConfigurationShouldSupportMultipleRegistrationsOfTheSameContract()
        {
            var cfg = new FluentContainerConfigurator();
            var reg1 = cfg.Register<IStub>().UsingConcreteType<Stub>();
            var reg2 = cfg.Register<IStub>().UsingConcreteType<StubWithDependency>();

            cfg.Registrations.Should().HaveCount(2).And.Contain(x => x.Equals(reg1)).And.Contain(x => x.Equals(reg2));
        }

        [Fact]
        public void RegistrationsOfSameTypeShouldNotEqualEachOther()
        {
            var cfg = new FluentContainerConfigurator();
            var reg1 = cfg.Register<IStub>().UsingConcreteType<Stub>();
            var reg2 = cfg.Register<IStub>().UsingConcreteType<StubWithDependency>();

            reg1.Should().NotBe(reg2);
        }

        [Fact]
        public void RegistrationShouldNotFailOnRegistrationButAfterContainerBeginsInitialization()
        {
            var cfg = new FluentContainerConfigurator();
            var reg1 = cfg.Register<IStub>().UsingConcreteType<StubWithMultipleCtors>();
            var reg2 = cfg.Register<IStubDependency>().UsingConcreteType<StubDependency>();
        }
    }
}