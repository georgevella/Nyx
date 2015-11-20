using System;
using System.Windows;
using Nyx.Presentation;

namespace Nyx.AppSupport
{
    public class WpfUserInterfaceThread : IUserInterfaceThread
    {
        private readonly Application _app;

        public WpfUserInterfaceThread(Application app)
        {
            _app = app;
        }

        public void Run(Action action)
        {
            if (_app.Dispatcher.CheckAccess())
            {
                action();
            }
            else
            {
                _app.Dispatcher.Invoke(action);
            }

        }
    }
}