using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace Nyx.AppSupport.Wpf.Dialogs
{
    public class BaseDialogCommand : FrameworkElement, ICommand
    {
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

        //////////////////////////////////////////////////////////////////////////

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

        //////////////////////////////////////////////////////////////////////////

        private void DoShowDialog(object parameter)
        {
            switch (this.ExecuteShowDialog(parameter))
            {
                case DialogResult.OK:
                case DialogResult.Yes:
                    if (AcceptCommand != null)
                    {
                        AcceptCommand.Execute(null);
                    }
                    break;
                default:
                    break;
            }
        }

        protected virtual DialogResult ExecuteShowDialog(object parameter)
        {
            return DialogResult.OK;
        }

        //////////////////////////////////////////////////////////////////////////

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        void ICommand.Execute(object parameter)
        {
            DoShowDialog(parameter);
        }

        public bool CanExecute(object parameter)
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