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
        public static IContainer Setup(Action<IContainerConfiguration> c)
        {
            if (c == null)
            {
                throw new ArgumentNullException(nameof(c));
            }
            var conf = new FluentContainerConfigurator();

            c(conf);

            var canister = new ContainerImpl(conf);
            canister.Build();
            return canister;
        }
    }
}