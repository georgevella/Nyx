using System.Windows;
using Nyx.AppSupport.Wpf;
using Nyx.AppSupport.Wpf.Commands;

namespace WpfSampleApplication.Commands
{
    public class ExitAppCommand : BaseCommand
    {
        private readonly IApplicationServices _app;

        public ExitAppCommand(IApplicationServices app)
        {
            _app = app;
        }

        public override void Execute()
        {
            _app.Exit();
        }

        public override bool CanExecute()
        {
            return true;
        }
    }
}