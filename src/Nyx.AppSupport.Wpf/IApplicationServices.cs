using System.Windows;

namespace Nyx.AppSupport
{
    public interface IApplicationServices
    {
        void Exit(int exitCode = 0);
    }

    class ApplicationServices : IApplicationServices
    {
        private readonly Application _app;

        public ApplicationServices(Application app)
        {
            _app = app;
        }

        public void Exit(int exitCode = 0)
        {
            _app.Shutdown(exitCode);
        }
    }
}