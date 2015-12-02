using System;

namespace Nyx.Presentation
{
    public interface IUserInterfaceThread
    {
        void Run(Action action);
    }
}