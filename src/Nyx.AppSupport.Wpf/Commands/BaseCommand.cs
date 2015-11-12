using System;
using System.Windows.Input;

namespace Nyx.AppSupport.Wpf.Commands
{
    public abstract class BaseCommand<T> : ICommand
    {
        void ICommand.Execute(object parameter)
        {
            Execute((T)parameter);
        }

        public abstract void Execute(T parameter);

        bool ICommand.CanExecute(object parameter)
        {
            return CanExecute((T)parameter);
        }

        public virtual bool CanExecute(T parameter)
        {
            return true;
        }
        public event EventHandler CanExecuteChanged;

        protected virtual void OnCanExecuteChanged()
        {
            var handler = CanExecuteChanged;
            if (handler != null) handler(this, EventArgs.Empty);
        }
    }

    public abstract class BaseCommand : ICommand
    {
        void ICommand.Execute(object parameter)
        {
            Execute();
        }

        public abstract void Execute();

        bool ICommand.CanExecute(object parameter)
        {
            return CanExecute();
        }

        public abstract bool CanExecute();

        public event EventHandler CanExecuteChanged;

        protected virtual void OnCanExecuteChanged()
        {
            var handler = CanExecuteChanged;
            if (handler != null) handler(this, EventArgs.Empty);
        }
    }
}