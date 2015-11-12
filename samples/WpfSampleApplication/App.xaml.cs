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
        protected override void OnStartup(StartupEventArgs e)
        {
            var bootstrapper = new AppBootstrapper(this);

            bootstrapper.Setup(c =>
            {
                c.UsingDefaultConventions().AutoDiscoverViewModels(In.ThisAssembly);

                c.AutoDiscoverCommands(In.ThisAssembly);

                c.Register<IUserNotificationService>().UsingConcreteType<MessageBoxNotificationService>();
            });

            bootstrapper.Start<MainViewModel>();
        }
    }
}
