using System;
using Nyx.Composition.Impl;

namespace Nyx.Composition
{
    /// <summary>
    /// </summary>
    public static class ContainerFactory
    {
        /// <summary>
        /// Creates and sets up a new instance Pyxis container
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="c"/> is <see langword="null" />.</exception>
        /// <exception cref="InvalidOperationException">Failed to setup container instance</exception>
        public static IContainer Setup(Action<IContainerConfiguration> c)
        {
            if (c == null)
            {
                throw new ArgumentNullException(nameof(c));
            }
            var conf = new FluentContainerConfigurator();

            try
            {
                c(conf);
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException("Failed to setup container instance", exception);
            }

            var container = new ContainerImpl(conf);

            // register container instance
            conf.Register<IContainer>().Using(container);
            conf.Register<IServiceProvider>().Using(container);

            container.Build();
            return container;
        }
    }
}