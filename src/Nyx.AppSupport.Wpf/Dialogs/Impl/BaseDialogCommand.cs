using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace Nyx.AppSupport.Dialogs.Impl
{
    internal abstract class BaseDialogCommand : DependencyObject, ICommand, INotifyPropertyChanged
    {
        #region AcceptCommand Dependency Property
        public static readonly DependencyProperty AcceptCommandProperty = DependencyProperty.RegisterAttached(
            "AcceptCommand",
            typeof(ICommand),
            typeof(BaseDialogCommand),
            new PropertyMetadata(null)
            );

        public static void SetAcceptCommand(FrameworkElement dObject, ICommand openCmd)
        {
            dObject.SetValue(AcceptCommandProperty, openCmd);
        }

        public static ICommand GetAcceptCommand(FrameworkElement dObject)
        {
            return dObject.GetValue(AcceptCommandProperty) as ICommand;
        }

        /// <summary>
        /// Defines a command which is triggered when the user presses the accept button on the dialog (i.e. the dialog returns DialogResult.Ok, or DialogResult.Yes)
        /// </summary>
        public ICommand AcceptCommand
        {
            get
            {
                return (ICommand)GetValue(AcceptCommandProperty);
            }
            set
            {
                SetValue(AcceptCommandProperty, value);
            }
        }
        #endregion

        protected abstract DialogResult ExecuteShowDialog(object parameter);

        protected virtual void OnDialogClosed(DialogResult dialogResult)
        {

        }

        protected virtual void OnDialogCancelled()
        {

        }

        protected virtual void OnDialogAccepted()
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



        void ICommand.Execute(object parameter)
        {
            var dialogResult = this.ExecuteShowDialog(parameter);

            OnDialogClosed(dialogResult);

            switch (dialogResult)
            {
                case DialogResult.OK:
                case DialogResult.Yes:
                    OnDialogAccepted();
                    AcceptCommand?.Execute(null);
                    break;
                default:
                    OnDialogCancelled();
                    break;
            }
        }

        public virtual bool CanExecute(object parameter)
        {
            return true;
        }

        #region Events
        public event EventHandler CanExecuteChanged;

        protected void InvokeCanExecuteChanged(EventArgs e)
        {
            var handler = CanExecuteChanged;
            if (handler != null) handler(this, e);
        }

        #endregion
    }
}