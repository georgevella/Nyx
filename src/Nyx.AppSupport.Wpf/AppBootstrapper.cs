using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Nyx.Composition;
using Nyx.Presentation;

namespace Nyx.AppSupport.Wpf
{
    public class AppBootstrapper
    {
        private readonly ViewResolver _viewResolver;

        public AppBootstrapper(Application app)
        {
            _viewResolver = new ViewResolver();
        }

        public void Start()
        {
            ContainerFactory.Setup(c =>
            {
                c.Register<IViewResolver>().Using(_viewResolver);

                foreach (var pair in _viewResolver)
                {
                    c.Register(pair.Key).UsingConcreteType(pair.Value);
                }
            });
        }
    }
}
