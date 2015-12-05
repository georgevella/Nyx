using System;
using System.Reflection;
using System.Windows;
using Nyx.Composition;
using Nyx.Presentation;
using Nyx.Presentation.Conventions;

namespace Nyx.AppSupport
{
    class NyxApplicationConfiguration : BaseNyxApplicationConfigurator
    {
        private readonly Application _app;


        public NyxApplicationConfiguration(Application app, IViewResolver viewResolver, IContainerConfiguration containerConfiguration) : base(viewResolver, containerConfiguration)
        {
            _app = app;
        }

        public override INyxApplicationConfiguration AutoDiscoverViewModels()
        {
            AutoDiscoverViewModelsImpl(_app.GetType().Assembly);
            return this;
        }

        public override INyxApplicationConfiguration AutoDiscoverCommands()
        {
            AutoDiscoverCommandsImpl(_app.GetType().Assembly);
            return this;
        }
    }
}