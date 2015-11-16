using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Nyx.AppSupport;
using Nyx.AppSupport.AppServices;
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

            var aboutMessageId = Guid.NewGuid();
            var exitMessageId = Guid.NewGuid();

            _bootstrapper = new AppBootstrapper(this);

            _bootstrapper.Setup(c =>
            {
                c.UsingDefaultConventions().AutoDiscoverViewModels(In.ThisAssembly);

                c.AutoDiscoverCommands(In.ThisAssembly);

                c.UsesSystemTray(s =>
                {
                    s.UsingIcon(new Uri("pack://application:,,,/Assets/Coherence.ico"))
                        .Menu(x =>
                        {
                            x.MenuItem("About", aboutMessageId)
                                .Seperator()
                                .MenuItem("Exit", exitMessageId);
                        });
                });
                c.UsesNotificationServices<MessageBoxNotificationService>();
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
