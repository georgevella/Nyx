using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Nyx.AppSupport.Wpf;
using Nyx.AppSupport.Wpf.AppServices;
using WpfSampleApplication.ViewModels;

namespace WpfSampleApplication
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private AppBootstrapper _bootstrapper;

        protected override void OnStartup(StartupEventArgs e)
        {

            _bootstrapper = new AppBootstrapper(this);

            _bootstrapper.Setup(c =>
            {
                c.UsingDefaultConventions().AutoDiscoverViewModels(In.ThisAssembly);

                c.AutoDiscoverCommands(In.ThisAssembly);

                c.Register<IUserNotificationService>().UsingConcreteType<MessageBoxNotificationService>();
            });

            _bootstrapper.Start<MainViewModel>();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _bootstrapper.Dispose();

            base.OnExit(e);
        }
    }
}
